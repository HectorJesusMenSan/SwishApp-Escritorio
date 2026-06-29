// =====================================================
// TorneoLogica.cs  —  Doble Eliminación escalable
// SIN recursión — usa bucle iterativo para BYEs.
// =====================================================

using System;
using System.Collections.Generic;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;

namespace SwishApp.Logica
{
    public class TorneoLogica
    {
        private readonly PartidoDao partidoDao = new PartidoDao();
        private readonly EquipoDao equipoDao = new EquipoDao();

        // =====================================================
        // FINALIZAR PARTIDO
        // =====================================================
        public void FinalizarPartido(Partido partido)
        {
            // Partido definitivo — no genera más
            if (partido.Bracket == "GRAN_FINAL_2")
            {
                int g = partido.PuntosA > partido.PuntosB
                    ? partido.IdEquipoA : partido.IdEquipoB;
                int p = partido.PuntosA > partido.PuntosB
                    ? partido.IdEquipoB : partido.IdEquipoA;
                SumarDerrota(p);
                Guardar(partido, g, p);
                return;
            }

            int idGanador = partido.PuntosA >= partido.PuntosB
                ? partido.IdEquipoA : partido.IdEquipoB;
            int idPerdedor = partido.PuntosA >= partido.PuntosB
                ? partido.IdEquipoB : partido.IdEquipoA;

            SumarDerrota(idPerdedor);
            Guardar(partido, idGanador, idPerdedor);

            if (partido.Bracket == "GRAN_FINAL")
                ProcesarGranFinal(partido.IdTorneo, idGanador, idPerdedor);
            else
                GenerarSiguientes(partido.IdTorneo);
        }

        // =====================================================
        // GRAN FINAL: ¿desempate?
        // =====================================================
        private void ProcesarGranFinal(
            int idTorneo, int idGanador, int idPerdedor)
        {
            if (idPerdedor == 0) return;

            var eqP = equipoDao.BuscarPorId(idPerdedor);
            if (eqP == null || eqP.Estado == "ELIMINADO") return;

            // El de Winners perdió → ambos con 1 derrota → desempate
            var partidos = partidoDao.ListarPorTorneo(idTorneo);
            if (!ExistePendiente(idGanador, idPerdedor, idTorneo, partidos))
            {
                Insertar(idTorneo, "GRAN_FINAL_2",
                    RondaMax(partidos) + 1,
                    idGanador, idPerdedor,
                    "Gran Final — Desempate");
            }
        }

        // =====================================================
        // MOTOR PRINCIPAL — bucle iterativo, sin recursión
        //
        // El bucle repite mientras se generen BYEs nuevos,
        // porque un BYE puede desbloquear un nuevo par.
        // Máximo de iteraciones = número de equipos (cota
        // segura que impide bucle infinito).
        // =====================================================
        private void GenerarSiguientes(int idTorneo)
        {
            int maxIter = 64; // cota de seguridad

            for (int iter = 0; iter < maxIter; iter++)
            {
                bool seInsertoBye = PasarUnaVez(idTorneo);

                // Si no se insertó ningún BYE nuevo en este paso,
                // el estado es estable → salir.
                if (!seInsertoBye) break;
            }
        }

        // =====================================================
        // UN PASO DEL MOTOR
        // Devuelve true si insertó al menos un BYE nuevo
        // (señal de que hay que volver a evaluar).
        // =====================================================
        private bool PasarUnaVez(int idTorneo)
        {
            var partidos = partidoDao.ListarPorTorneo(idTorneo);
            var equipos = equipoDao.ListarPorTorneo(idTorneo);

            // ── IDs ya en PENDIENTE ───────────────────────
            var ocupados = new HashSet<int>();
            foreach (var p in partidos)
            {
                if (p.Estado != "PENDIENTE") continue;
                if (p.IdEquipoA > 0) ocupados.Add(p.IdEquipoA);
                if (p.IdEquipoB > 0) ocupados.Add(p.IdEquipoB);
            }

            // ── Libres por bracket ────────────────────────
            var libresW = new List<int>();
            var libresL = new List<int>();
            int totalActivos = 0;

            foreach (var eq in equipos)
            {
                if (eq.Estado == "ELIMINADO") continue;
                totalActivos++;
                if (ocupados.Contains(eq.Id)) continue;
                if (eq.Derrotas == 0) libresW.Add(eq.Id);
                else if (eq.Derrotas == 1) libresL.Add(eq.Id);
            }

            // ── Ronda actual y pendientes por bracket ─────
            int rondaW = RondaMaxBracket(partidos, "WINNERS");
            int rondaL = RondaMaxBracket(partidos, "LOSERS");

            bool pendW = HayPendientesEnRonda(partidos, "WINNERS", rondaW);
            bool pendL = HayPendientesEnRonda(partidos, "LOSERS", rondaL);

            // ── Totales activos por bracket ───────────────
            int totalW = 0, totalL = 0;
            foreach (var eq in equipos)
            {
                if (eq.Estado != "ACTIVO") continue;
                if (eq.Derrotas == 0) totalW++;
                else if (eq.Derrotas == 1) totalL++;
            }

            // ── GRAN FINAL ────────────────────────────────
            if (totalW == 1 && totalL == 1 && !pendW && !pendL)
            {
                int? idW = null, idL = null;
                foreach (var eq in equipos)
                {
                    if (eq.Estado != "ACTIVO") continue;
                    if (eq.Derrotas == 0) idW = eq.Id;
                    if (eq.Derrotas == 1) idL = eq.Id;
                }

                if (idW.HasValue && idL.HasValue
                    && !ocupados.Contains(idW.Value)
                    && !ocupados.Contains(idL.Value)
                    && !ExistePendiente(idW.Value, idL.Value,
                           idTorneo, partidos))
                {
                    Insertar(idTorneo, "GRAN_FINAL",
                        Math.Max(rondaW, rondaL) + 1,
                        idW.Value, idL.Value, "Gran Final");
                }
                return false; // estable
            }

            bool byeInsertado = false;

            // ── WINNERS ───────────────────────────────────
            if (!pendW)
            {
                if (libresW.Count >= 2)
                {
                    bool inserto = EmparejarRonda(
                        libresW, idTorneo, "WINNERS",
                        rondaW + 1, partidos);

                    if (inserto) return false; // generó partidos normales, estable
                }
                else if (libresW.Count == 1 && totalL > 1)
                {
                    // Solo 1 en W y Losers tiene varios activos
                    // → BYE para que no quede bloqueado
                    int sobrante = libresW[0];
                    if (!TieneBye(partidos, "WINNERS", rondaW + 1, sobrante))
                    {
                        InsertarBye(idTorneo, "WINNERS", rondaW + 1, sobrante);
                        byeInsertado = true;
                    }
                }
            }

            // ── LOSERS ────────────────────────────────────
            // Re-leer con datos frescos (puede haber cambiado
            // si acabamos de insertar un BYE en Winners)
            if (byeInsertado)
            {
                partidos = partidoDao.ListarPorTorneo(idTorneo);
                ocupados.Clear();
                foreach (var p in partidos)
                {
                    if (p.Estado != "PENDIENTE") continue;
                    if (p.IdEquipoA > 0) ocupados.Add(p.IdEquipoA);
                    if (p.IdEquipoB > 0) ocupados.Add(p.IdEquipoB);
                }
                libresL.Clear();
                foreach (var eq in equipoDao.ListarPorTorneo(idTorneo))
                {
                    if (eq.Estado == "ELIMINADO") continue;
                    if (ocupados.Contains(eq.Id)) continue;
                    if (eq.Derrotas == 1) libresL.Add(eq.Id);
                }
                rondaL = RondaMaxBracket(partidos, "LOSERS");
                pendL = HayPendientesEnRonda(partidos, "LOSERS", rondaL);
            }

            if (!pendL && libresL.Count >= 2)
            {
                bool inserto = EmparejarRonda(
                    libresL, idTorneo, "LOSERS",
                    rondaL + 1, partidos);

                if (inserto) byeInsertado = false; // partidos normales, no BYE
            }
            else if (!pendL && libresL.Count == 1 && totalW > 1)
            {
                // 1 solo en L, aún hay varios en Winners
                int sobrante = libresL[0];
                if (!TieneBye(partidos, "LOSERS", rondaL + 1, sobrante))
                {
                    InsertarBye(idTorneo, "LOSERS", rondaL + 1, sobrante);
                    byeInsertado = true;
                }
            }

            return byeInsertado;
        }

        // =====================================================
        // EMPAREJAR RONDA
        // Crea los partidos de 2 en 2. Si queda impar,
        // inserta BYE y devuelve true (hay que re-evaluar).
        // Devuelve true si insertó algo nuevo.
        // =====================================================
        private bool EmparejarRonda(
            List<int> libres, int idTorneo,
            string bracket, int ronda,
            List<Partido> partidos)
        {
            bool insertado = false;

            for (int i = 0; i + 1 < libres.Count; i += 2)
            {
                int eqA = libres[i];
                int eqB = libres[i + 1];

                if (!ExistePendiente(eqA, eqB, idTorneo, partidos))
                {
                    Insertar(idTorneo, bracket, ronda,
                        eqA, eqB, bracket + " R" + ronda);
                    insertado = true;
                }
            }

            // Sobrante impar → BYE (FINALIZADO inmediato)
            if (libres.Count % 2 != 0)
            {
                int sobrante = libres[libres.Count - 1];
                if (!TieneBye(partidos, bracket, ronda, sobrante))
                {
                    InsertarBye(idTorneo, bracket, ronda, sobrante);
                    insertado = true;
                }
            }

            return insertado;
        }

        // =====================================================
        // INSERTAR PARTIDO NORMAL
        // =====================================================
        private void Insertar(
            int idTorneo, string bracket, int ronda,
            int idA, int idB, string nombre)
        {
            partidoDao.Insertar(new Partido
            {
                Nombre = nombre,
                Estado = "PENDIENTE",
                PuntosA = 0,
                PuntosB = 0,
                Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                Bracket = bracket,
                Ronda = ronda,
                IdEquipoA = idA,
                IdEquipoB = idB,
                IdTorneo = idTorneo,
                Bye = false,
                Ganador = 0,
                Perdedor = 0
            });
        }

        // =====================================================
        // INSERTAR BYE — siempre FINALIZADO al nacer
        // =====================================================
        private void InsertarBye(
            int idTorneo, string bracket, int ronda, int idEquipo)
        {
            var eq = equipoDao.BuscarPorId(idEquipo);
            string nombre = (eq?.Nombre ?? "Equipo") + " — BYE";

            partidoDao.Insertar(new Partido
            {
                Nombre = nombre,
                Estado = "FINALIZADO",
                PuntosA = 0,
                PuntosB = 0,
                Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                Bracket = bracket,
                Ronda = ronda,
                IdEquipoA = idEquipo,
                IdEquipoB = 0,
                IdTorneo = idTorneo,
                Bye = true,
                Ganador = idEquipo,
                Perdedor = 0
            });
        }

        // =====================================================
        // SUMAR DERROTA
        // =====================================================
        private void SumarDerrota(int idEquipo)
        {
            if (idEquipo <= 0) return;
            var eq = equipoDao.BuscarPorId(idEquipo);
            if (eq == null) return;
            eq.Derrotas++;
            eq.Estado = eq.Derrotas >= 2 ? "ELIMINADO" : "ACTIVO";
            equipoDao.ActualizarDerrotas(eq);
        }

        // =====================================================
        // GUARDAR RESULTADO
        // =====================================================
        private void Guardar(Partido p, int ganador, int perdedor)
        {
            p.Estado = "FINALIZADO";
            p.Ganador = ganador;
            p.Perdedor = perdedor;
            partidoDao.Actualizar(p);
        }

        // =====================================================
        // HELPERS
        // =====================================================

        private bool HayPendientesEnRonda(
            List<Partido> partidos, string bracket, int ronda)
        {
            foreach (var p in partidos)
                if (p.Bracket == bracket
                    && p.Ronda == ronda
                    && p.Estado == "PENDIENTE")
                    return true;
            return false;
        }

        private int RondaMaxBracket(List<Partido> partidos, string bracket)
        {
            int max = 0;
            foreach (var p in partidos)
                if (p.Bracket == bracket && p.Ronda > max)
                    max = p.Ronda;
            return max;
        }

        private int RondaMax(List<Partido> partidos)
        {
            int max = 0;
            foreach (var p in partidos)
                if (p.Ronda > max) max = p.Ronda;
            return max;
        }

        private bool ExistePendiente(
            int idA, int idB, int idTorneo,
            List<Partido> partidos, string bracket = null)
        {
            foreach (var p in partidos)
            {
                if (p.IdTorneo != idTorneo) continue;
                if (p.Estado == "FINALIZADO") continue;
                if (bracket != null && p.Bracket != bracket) continue;

                bool coincide =
                    (p.IdEquipoA == idA && p.IdEquipoB == idB) ||
                    (p.IdEquipoA == idB && p.IdEquipoB == idA);

                if (coincide) return true;
            }
            return false;
        }

        private bool TieneBye(
            List<Partido> partidos, string bracket, int ronda, int idEquipo)
        {
            foreach (var p in partidos)
            {
                if (!p.Bye) continue;
                if (p.Bracket != bracket) continue;
                if (p.Ronda != ronda) continue;
                if (p.IdEquipoA == idEquipo || p.IdEquipoB == idEquipo)
                    return true;
            }
            return false;
        }
    }
}