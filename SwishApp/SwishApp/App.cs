using SwishApp.Modelo;

namespace SwishApp
{
    public static class App
    {
        // Torneo activo en la sesión actual
        public static int IdTorneoActivo { get; set; }

        // Usuario logueado
        public static Usuario UsuarioActivo { get; set; }
    }
}