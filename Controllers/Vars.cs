public class Vars{
    private readonly string cadenaConexion = @"Server=localhost;Database=control_notas;Trusted_Connection=True;";

    public object ErrorGenerico(){
        return new {error = true, msj = "Ha ocurrido un error"};
    }

    public object Exito(string _msj){
        return new {error = false, msj = _msj};
    }

    public string CadenaConexion(){
        return cadenaConexion;
    }
}