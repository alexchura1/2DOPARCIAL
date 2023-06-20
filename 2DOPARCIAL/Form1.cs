using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace _2DOPARCIAL
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=LAPTOP-OOV5EART\\XE;Database=BD_IDS;Trusted_Connection=True;";
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionString);
            dataAdapter = new SqlDataAdapter("SELECT * FROM NOTAS", connection);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dgvDatos.DataSource = dataTable;

            // Suscribir el evento TextChanged al método txtBuscar_TextChanged
            txtBuscar.TextChanged += txtBuscar_TextChanged;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtBuscar.Text.Trim();
            MostrarResultadosBusqueda(searchTerm);
        }

        private void MostrarResultadosBusqueda(string searchTerm)
        {
            DataTable dataTable = BuscarRegistros(searchTerm);
            dgvDatos.DataSource = dataTable;
        }

        private DataTable BuscarRegistros(string searchTerm)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string selectQuery = "SELECT * FROM NOTAS WHERE materia LIKE '%' + @searchTerm + '%';";
                SqlCommand command = new SqlCommand(selectQuery, conn);
                command.Parameters.AddWithValue("@searchTerm", searchTerm);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                conn.Close();

                return dataTable;
            }
        }

        private void pbCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMat.Text))
            {
                string materia = txtMat.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string insertQuery = "INSERT INTO NOTAS (materia, sigla) VALUES (@materia, @sigla);";
                        SqlCommand command = new SqlCommand(insertQuery, conn);
                        command.Parameters.AddWithValue("@materia", materia);
                        command.Parameters.AddWithValue("@sigla", materia);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Datos insertados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al insertar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Materia no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMod_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCod.Text) && !string.IsNullOrEmpty(txtMat.Text))
            {
                string codigo = txtCod.Text;
                string nuevaMateria = txtMat.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string updateQuery = "UPDATE NOTAS SET materia = @nuevaMateria, sigla = @sigla WHERE nota_nro = @codigo;";
                        SqlCommand command = new SqlCommand(updateQuery, conn);
                        command.Parameters.AddWithValue("@nuevaMateria", nuevaMateria);
                        command.Parameters.AddWithValue("@sigla", nuevaMateria);
                        command.Parameters.AddWithValue("@codigo", codigo);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Datos actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Código y materia no pueden estar vacíos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCod.Text))
            {
                string codigo = txtCod.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM NOTAS WHERE nota_nro = @codigo;";
                        SqlCommand command = new SqlCommand(deleteQuery, conn);
                        command.Parameters.AddWithValue("@codigo", codigo);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Registro eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el registro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Código no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
