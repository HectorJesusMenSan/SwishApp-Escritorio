using System;
using System.Collections.Generic;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;

namespace SwishApp.Logica
{
    public class TorneoLogica
    {
        // ================= DAOs =================
        private PartidoDao partidoDao = new PartidoDao();
        private EquipoDao equipoDao = new EquipoDao();

        // =====================================================
        // GENERAR SIGUIENTE PARTIDO
        // =====================================================
        public void GenerarSiguientePartido(Partido partidoFinalizado)
        {
            int idTorneo = partidoFinalizado.IdTorneo;

            if (partidoFinalizado.Bracket == "GRAN_FINAL")
                return;

            var partidos = partidoDao.ListarPorTorneo(idTorneo);

            // Equipos con partido pendiente
            var ocupados = new List<int>();

            foreach (var p in partidos)
            {
                if (p.Estado != "PENDIENTE") continue;

                if (p.IdEquipoA != 0) ocupados.Add(p.IdEquipoA);
                if (p.IdEquipoB != 0) ocupados.Add(p.IdEquipoB);
            }

            // Equipos activos en WINNERS (0 derrotas)
            var disponiblesWinners = new List<int>();

            // Equipos activos en LOSERS (1 derrota)
            var disponiblesLosers = new List<int>();

            var todosEquipos = equipoDao.ListarPorTorneo(idTorneo);

            foreach (var e in todosEquipos)
            {
                if (e.Estado != "ACTIVO") continue;
                if (ocupados.Contains(e.Id)) continue;

                if (e.Derrotas == 0)
                    disponiblesWinners.Add(e.Id);
                else if (e.Derrotas == 1)
                    disponiblesLosers.Add(e.Id);
            }

            // Calcular siguiente ronda
            int siguienteRonda = 1;

            foreach (var p in partidos)
            {
                if (p.Bracket == "GRAN_FINAL") continue;
                if (p.Ronda >= siguienteRonda)
                    siguienteRonda = p.Ronda + 1;
            }

            // =========================================
            // GRAN FINAL
            // =========================================
            if (disponiblesWinners.Count == 1
                && disponiblesLosers.Count == 1)
            {
                bool hayPendientes = false;

                foreach (var p in partidos)
                {
                    if (p.Bracket == "GRAN_FINAL") continue;
                    if (p.Estado == "PENDIENTE")
                    {
                        hayPendientes = true;
                        break;
                    }
                }

                if (!hayPendientes)
                {
                    bool existe = partidoDao.ExistePartidoPendiente(
                        disponiblesWinners[0],
                        disponiblesLosers[0],
                        idTorneo
                    );

                    if (!existe)
                    {
                        var granFinal = new Partido
                        {
                            Nombre = "Gran Final",
                            Estado = "PENDIENTE",
                            PuntosA = 0,
                            PuntosB = 0,
                            Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                            Bracket = "GRAN_FINAL",
                            Ronda = siguienteRonda,
                            IdEquipoA = disponiblesWinners[0],
                            IdEquipoB = disponiblesLosers[0],
                            IdTorneo = idTorneo
                        };

                        partidoDao.Insertar(granFinal);
                    }
                }

                return;
            }

            // =========================================
            // PARTIDOS WINNERS
            // =========================================
            for (int i = 0; i < disponiblesWinners.Count - 1; i += 2)
            {
                int eqA = disponiblesWinners[i];
                int eqB = disponiblesWinners[i + 1];

                if (eqA == eqB) continue;

                bool existe = partidoDao.ExistePartidoPendiente(
                    eqA, eqB, idTorneo
                );

                if (!existe)
                {
                    partidoDao.Insertar(new Partido
                    {
                        Nombre = "WINNERS Ronda " + siguienteRonda,
                        Estado = "PENDIENTE",
                        PuntosA = 0,
                        PuntosB = 0,
                        Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                        Bracket = "WINNERS",
                        Ronda = siguienteRonda,
                        IdEquipoA = eqA,
                        IdEquipoB = eqB,
                        IdTorneo = idTorneo
                    });
                }
            }

            // =========================================
            // PARTIDOS LOSERS
            // =========================================
            for (int i = 0; i < disponiblesLosers.Count - 1; i += 2)
            {
                int eqA = disponiblesLosers[i];
                int eqB = disponiblesLosers[i + 1];

                if (eqA == eqB) continue;

                bool existe = partidoDao.ExistePartidoPendiente(
                    eqA, eqB, idTorneo
                );

                if (!existe)
                {
                    partidoDao.Insertar(new Partido
                    {
                        Nombre = "LOSERS Ronda " + siguienteRonda,
                        Estado = "PENDIENTE",
                        PuntosA = 0,
                        PuntosB = 0,
                        Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                        Bracket = "LOSERS",
                        Ronda = siguienteRonda,
                        IdEquipoA = eqA,
                        IdEquipoB = eqB,
                        IdTorneo = idTorneo
                    });
                }
            }
        }

        // =====================================================
        // FINALIZAR PARTIDO
        // =====================================================
        public void FinalizarPartido(Partido partido)
        {
            // Definir ganador y perdedor
            int ganador, perdedor;

            if (partido.PuntosA > partido.PuntosB)
            {
                ganador = partido.IdEquipoA;
                perdedor = partido.IdEquipoB;
            }
            else
            {
                ganador = partido.IdEquipoB;
                perdedor = partido.IdEquipoA;
            }

            // Actualizar derrotas del perdedor
            var equipoPerdedor = equipoDao.BuscarPorId(perdedor);

            equipoPerdedor.Derrotas++;

            equipoPerdedor.Estado = equipoPerdedor.Derrotas >= 2
                ? "ELIMINADO"
                : "ACTIVO";

            equipoDao.ActualizarDerrotas(equipoPerdedor);

            // Guardar partido
            partido.Estado = "FINALIZADO";
            partido.Ganador = ganador;
            partido.Perdedor = perdedor;

            partidoDao.Actualizar(partido);

            // Generar siguiente partido
            GenerarSiguientePartido(partido);
        }
    }
}