using Elibom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Vehitanques.BLL;
using WPF_Vehitanques.DS;
using WPF_Vehitanques.Presentacion.Controles;
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para EnvioMsgDeTexto.xaml
    /// </summary>
    public partial class EnvioMsgDeTexto : Window
    {
        #region Declaraciones
        DSMensajesTexto.DSPlacasMSGDataTable DSplacas = new DSMensajesTexto.DSPlacasMSGDataTable();
        VentanaMensaje Venmsj;
        MensajeBLL Mensabll = new MensajeBLL();
        MSGBLL msgbll = new MSGBLL();
        ProgramadosMantenimiento ProgMan;
        private string usuariomsg = "";
        UserMsg UsrMsg = new UserMsg();
        int idArea=2;
        DS.DSMensajesTextoTableAdapters.Vehit_GuardacopiamsgmantenimientoTableAdapter DScopiamante = new DS.DSMensajesTextoTableAdapters.Vehit_GuardacopiamsgmantenimientoTableAdapter();
        #endregion
        #region Load
        public EnvioMsgDeTexto()
        {
            InitializeComponent();
        }
        #endregion
        #region Métodos
        private Boolean validarCaracteresespeciales(KeyEventArgs e, TextBox txt)
        {
            Boolean respuesta;
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                MessageBox.Show(" la combinación de esta tecla no está pérmitida " + e.Key.ToString());
                txt.Text = "";
                respuesta = false;
            }
            else
            {
                respuesta = true;
            }
            return respuesta;
        }
        public void GetDatasetDatos(DSMensajesTexto.DSPlacasMSGDataTable ds, string usuario,int _idarea)
        {
            DSplacas = ds;
            usuariomsg = usuario;
            idArea = _idarea;
        }
        public void GetMain(ProgramadosMantenimiento ventapen)
        {
            ProgMan = ventapen;
        }
        #endregion
        #region Eventos
        private void txtmensajetexto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (txt.Text.Length <= 150)
                {
                    int faltante = 150 - txt.Text.Length;
                    lblerror_mensaje.Content = "Cantidad letras: " + faltante;
                }
            }
        }
        private void btn_EnviarMsg_Click(object sender, RoutedEventArgs e)
        {

            ElibomClient elibom = new ElibomClient(UsrMsg.GetUsuario(), UsrMsg.GetPassword());
            var listacuenta = elibom.getAccount();
            var creditos = listacuenta["credits"];
            var cuenta = listacuenta["name"];
            var tipoCuenta = listacuenta["type"];
            int credi = Convert.ToInt32(creditos);
            if (credi < DSplacas.Count)
            {
                MessageBox.Show("No tiene Saldo suficiente para enviar estos mensajes");
                this.Close();
            }
            if (txtmensajetexto.Text.Trim() != "")
            {
                if (txtmensajetexto.Text.Trim().Length > 150)
                {
                    lblerror_mensaje.Content = "el mensaje es demasiado largo ";
                    return;
                }
                else
                {
                    String pattern = "[a-zA-Z01]?";
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
                    if (regex.IsMatch(txtmensajetexto.Text))
                    {
                        string numeros = "";
                        string placas = "";
                        int i = 0;
                        var lista = msgbll.GetNumerosCopia(idArea);
                        foreach (var item in lista)
                        {
                            DSplacas.AddDSPlacasMSGRow(item.NumC_Nombre, item.NumC_Numero,"");
                        }
                        foreach (var itemcel in DSplacas)
                        {
                            i = i + 1;
                            if (itemcel.Celular != "")
                            {
                                if (DSplacas.Count > 1 & i < DSplacas.Count)
                                {
                                    numeros = numeros + itemcel.Celular + ",";
                                }
                                else
                                {
                                    numeros = numeros + itemcel.Celular;
                                }
                            }
                            else
                            {
                                placas = placas + "," + itemcel.Placa;
                            }

                            try
                            {
                                DScopiamante.Get_Vehit_Guardacopiamsgmantenimiento(txtmensajetexto.Text.Trim(), itemcel.Placa, itemcel.Celular, itemcel.IDConductor);
                            }
                            catch
                            {

                            }                            
                        }
                        try
                        {
                            var delivery = elibom.sendMessage(numeros, txtmensajetexto.Text.Trim());
                            var listadelivery = elibom.getDelivery(delivery.ToString());
                            var fallos = listadelivery["numFailed"];
                            int cantidad_fallos = 0;
                            try
                            {
                                cantidad_fallos = Convert.ToInt32(listadelivery["numFailed"]);
                            }
                            catch
                            {
                                cantidad_fallos = 0;
                            }
                            var mens = elibom.GetHashCode();
                            if (usuariomsg == "")
                            {
                                usuariomsg = "UsrNoRegistra";
                            }
                            msgbll.GuardarHisroicoResponseMsg(delivery.ToString(), txtmensajetexto.Text.Trim(), usuariomsg, (DSplacas.Count - cantidad_fallos), idArea);
                            string mensaje = ""; if (cantidad_fallos > 0)
                            {
                                mensaje = ", Pero fallo en " + cantidad_fallos + ", de la cantidad enviada";
                            }
                            Venmsj = new VentanaMensaje();
                            Mensabll.tipomensajes(59,"");
                            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes + mensaje);
                            Venmsj.ShowDialog();
                            if (ProgMan != null)
                            {
                                ProgMan.limpiargrillacheCkpoints();
                            }
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            string mensaje = ex.Message.ToString();
                            char[] delimiters = new char[] { '(', ')' };
                            string[] parts = mensaje.Split(delimiters,
                                             StringSplitOptions.RemoveEmptyEntries);
                            msgbll.GuardarErroresMsg(mensaje, Convert.ToInt16(parts[1]));
                            Venmsj = new VentanaMensaje();
                            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            Venmsj.cargarmensaje(ex.ToString(), ex.Message.ToString());
                            Venmsj.ShowDialog();
                            this.Close();
                        }
                        // MessageBox.Show("Correcto");
                    }
                    else
                    {
                        MessageBox.Show("El texto contiene caracteres no válidos");
                        return;
                    }

                }
            }
            else
            {
                //lblerror_mensaje.Content = "Debe diligenciar el mensaje de texto antes de enviar";
                MessageBox.Show("Debe diligenciar el mensaje de texto antes de enviar");
            }
        }
        #endregion





    }
}
