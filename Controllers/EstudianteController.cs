using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace backend_cursos.Controllers;

[ApiController]
[Route("[controller]")]
public class EstudianteController : ControllerBase {
    Vars v = new();

    [HttpGet("ObtenerEstudiantes")]
    public IActionResult ObtenerEstudiantes(){
        try{
            List<Estudiante> estudiantes = new();
            string sql = "select id,nombre,activo from estudiante";
            SqlDataReader reader;
            SqlConnection conexion = new(v.CadenaConexion());
            SqlCommand cmd = new(sql,conexion);

            conexion.Open();

            reader = cmd.ExecuteReader();

            if(reader.HasRows){
                while(reader.Read()){
                    _ = int.TryParse(reader["id"].ToString(), out int _id);
                    string? _nombre = reader["nombre"].ToString();
                    _ = bool.TryParse(reader["activo"].ToString(), out bool _activo);
                    estudiantes.Add(new Estudiante{id = _id, nombre = _nombre, activo = _activo});
                }
                return Ok(estudiantes);
            }
            else{
                var respuesta = new { error = true, msj = "No se encontraron registros"};
                return Ok(respuesta);
            }
            
        }
        catch(Exception ex){
            Console.WriteLine("Error: " + ex.Message);
            return StatusCode(500, v.ErrorGenerico());
        }
    }

    [HttpGet("ObtenerEstudiante/{id}",Name ="ObtenerEstudiantePorId")]
    public IActionResult ObtenerEstudiante(int id){
        try{
            string sql = "select id,nombre,activo from estudiante where id = @id";
            SqlDataReader reader;
            SqlConnection conexion = new(v.CadenaConexion());
            SqlCommand cmd = new(sql,conexion);
            Estudiante estudiante = new Estudiante{id = 0, nombre = ""};
            
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = id;

            conexion.Open();

            reader = cmd.ExecuteReader();

            if (reader.HasRows){
                reader.Read();
                _ = int.TryParse(reader["id"].ToString(), out int _id);
                string? _nombre = reader["nombre"].ToString();
                _ = bool.TryParse(reader["activo"].ToString(), out bool _activo);
                estudiante = new Estudiante{id = _id, nombre = _nombre, activo = _activo};
                conexion.Close();
                return Ok(estudiante);
            }
            else {
                conexion.Close();
                var respuesta = new { error = true, msj = "No se encontró al estudiante" };
                return Ok(respuesta);
            }
        }
        catch (Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return BadRequest(v.ErrorGenerico());
        }
    }

    [HttpPost("GuardarEstudiante")]
    public IActionResult GuardarEstudiante(Estudiante estudiante){
        try{
            string sql = "insert into estudiante(nombre, activo) values(@nombre,@activo)";
            SqlConnection conexion = new(v.CadenaConexion());
            SqlCommand cmd = new(sql,conexion);

            cmd.Parameters.Add("@nombre",SqlDbType.NVarChar);
            cmd.Parameters.Add("@activo",SqlDbType.Bit);

            cmd.Parameters["@nombre"].Value = estudiante.nombre;
            cmd.Parameters["@activo"].Value = estudiante.activo;
            
            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            return Ok(v.Exito("Se guardó correctamente al estudiante"));
        }
        catch (Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return BadRequest(v.ErrorGenerico());
        }
    }

    [HttpPut("ActualizarEstudiante")]
    public IActionResult ActualizarEstudiante(Estudiante estudiante){
        try{
            int total = 0;
            string sql = "update estudiante set nombre = @nombre, activo = @activo where id = @id";
            SqlConnection conexion = new(v.CadenaConexion());
            SqlCommand cmd = new(sql,conexion);

            cmd.Parameters.Add("@id",SqlDbType.Int);
            cmd.Parameters.Add("@nombre",SqlDbType.NVarChar);
            cmd.Parameters.Add("@activo",SqlDbType.Bit);

            cmd.Parameters["@id"].Value = estudiante.id;
            cmd.Parameters["@nombre"].Value = estudiante.nombre;
            cmd.Parameters["@activo"].Value = estudiante.activo;

            conexion.Open();
            total = cmd.ExecuteNonQuery();
            conexion.Close();

            if(total > 0){
                return Ok(v.Exito("Se actualizó correctamente el estudiante"));
            }
            else{
                return Ok(v.Exito("No se encontró al estudiante que se desea actualizar"));
            }
        }
        catch (Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return BadRequest(v.ErrorGenerico());
        }
    }

    [HttpDelete("EliminarEstudiante/{id}")]
    public IActionResult EliminarEstudiante(int id){
        try{
            string sql = "delete from estudiante where id = @id";
            SqlConnection conexion = new(v.CadenaConexion());
            SqlCommand cmd = new(sql,conexion);
            int total = 0;

            cmd.Parameters.Add("@id",SqlDbType.Int);
        
            cmd.Parameters["@id"].Value = id;
            
            conexion.Open();
            total = cmd.ExecuteNonQuery();
            conexion.Close();

            if(total > 0){
                return Ok(v.Exito("Se eliminó correctamente al estudiante"));
            }
            else{
                return Ok(v.Exito("No se encontró al estudiante que se desea eliminar"));
            }
        }
        catch (Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return BadRequest(v.ErrorGenerico());
        }
    }
}