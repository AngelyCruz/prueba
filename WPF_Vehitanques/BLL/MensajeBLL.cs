using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Vehitanques.BLL
{

    public class MensajeBLL
    {
        private string titulo, mensaje;
        public bool boolmensaje = true;

        public String titulos
        {
            get { return titulo; }
            set { titulo = value; }

        }
        public String mensajes
        {
            get { return mensaje; }
            set { mensaje = value; }

        }
        public bool boolmensajes
        {
            get { return boolmensaje; }
            set { boolmensaje = value; }

        }
        public void tipomensajes(int tipomensaje, string strComplemento)
        {
            switch (tipomensaje)
            {
                case 1:
                    titulos = "Alerta";
                    mensajes = "Salvar datos antes de salir";
                    break;
                case 2:
                    titulos = "Advertencia";
                    mensajes = "No existen datos para ésta búsqueda";
                    break;
                case 3:
                    titulos = "Mensaje";
                    mensajes = "No existen datos de linea para ésta marca";
                    break;
                case 4:
                    titulos = "Alerta";
                    mensajes = "La Linea de este vehículo no Existe por favor seleccionar una linea correspondiente a la marca";
                    break;
                case 5:
                    titulos = "Alerta";
                    mensajes = "La fecha de vencimiento quinta rueda está vencida";
                    break;
                case 6:
                    titulos = "Alerta";
                    mensajes = "La fecha de revisión técnico mecánica está vencida";
                    break;
                case 7:
                    titulos = "Alerta";
                    mensajes = "La fecha del SOAT está vencida";
                    break;
                case 8:
                    titulos = "Alerta";
                    mensajes = "La fecha de la póliza RC está vencida";
                    break;
                case 9:
                    titulos = "Alerta";
                    mensajes = "La unidad de medida del remolque no es la adecuada por favor seleccionar una medida adecuada";
                    break;
                case 10:
                    titulos = "El Nombre Del Archivo Es Demasiado Grande";
                    mensajes = "El Nombre del archivo es demasiado grande";
                    break;
                case 11:
                    titulos = "El Peso del archivo es Exede  el límite permitido de ";
                    mensajes = " Megas";
                    break;
                case 12:
                    titulos = "Mensaje";
                    mensajes = "Datos Salvados Satisfactoriamente";
                    break;
                case 13:
                    titulos = "Advertencia";
                    mensajes = "La fecha de vigencia de este documento no puede ser superior a un año";
                    break;
                case 14:
                    titulos = "Advertencia";
                    mensajes = "La fecha no puede ser inferior a la fecha actual";
                    break;
                case 15:
                    titulos = "Advertencia";
                    mensajes = "La fecha no puede ser superior a la fecha actual";
                    break;
                case 16:
                    titulos = "Advertencia";
                    mensajes = "La fecha de vigencia de este documento no puede ser superior a tres años";
                    break;
                case 17:
                    titulos = "Advertencia";
                    mensajes = "Para ingresar un modelo repotenciado primero debe ingresar el modelo del vehículo";
                    break;
                case 18:
                    titulos = "Advertencia";
                    mensajes = "El modelo repotenciado no puede ser superior al año actual";
                    break;
                case 19:
                    titulos = "Advertencia";
                    mensajes = "El modelo repotenciado no puede ser inferior o igual al modelo del vehículo";
                    break;
                case 20:
                    titulos = "Advertencia";
                    mensajes = "El modelo  debe ser superior a 1900";
                    break;
                case 21:
                    titulos = "Advertencia";
                    mensajes = "El modelo  sólo puede ser un año superior al año actual";
                    break;
                case 22:
                    titulos = "Mensaje";
                    mensajes = "La placa del vehículo ya existe en el sistema";
                    break;
                case 23:
                    titulos = "Error";
                    mensajes = "Se presentó un eror al salvar la información por favor comunicarse con el administrador del sistema";
                    break;
                case 24:
                    titulos = "Alerta";
                    mensajes = "El conductor no registra una ciudad de residencia válida";
                    break;
                case 25:
                    titulos = "Alerta";
                    mensajes = "El beneficiario no registra una agencia válida";
                    break;
                case 26:
                    titulos = "Alerta";
                    mensajes = "La Clase del remolque no corresponde al tipo de carrocería del vehículo";
                    break;
                case 27:
                    titulos = "Alerta";
                    mensajes = "La marca se encuentra en un formato no válido, por favor seleccione una marca correcta";
                    break;
                case 28:
                    titulos = "Alerta";
                    //mensajes = "Este documento Ha sido eliminado de su ubicación, comuníquese con el administrador del sistema";
                    mensajes = "Este documento ha sido eliminado de su ubicación, se inactivará el registro del documento para que lo cargue de nuevo";
                    break;
                case 29:
                    titulos = "Error";
                    mensajes = "Se presentó un error en la búsqueda de la información";
                    break;
                case 30:
                    titulos = "Alerta";
                    mensajes = "El número de póliza ya Existe, No se Permite su Duplicación";
                    break;
                case 31:
                    titulos = "Alerta";
                    mensajes = "No se puede Asignar una póliza a la placa: ";
                    break;
                case 32:
                    titulos = "mensaje";
                    mensajes = "Este documento solo se puede adjuntar a vehículos tipo tanque ";
                    break;
                case 33:
                    titulos = "Alerta";
                    mensajes = "Este Contrato ya Existe para la placa: ";
                    break;
                case 34:
                    titulos = "Alerta";
                    mensajes = "Este Vehículo no se puede activar ya que tiene documentación pendiente por favor revisar el boton de alertas ";
                    break;
                case 35:
                    titulos = "Alerta";
                    mensajes = "Este Número Nit ya Existe Para Otra Compañía";
                    break;
                case 36:
                    titulos = "Alerta Usuario: ";
                    mensajes = "Usted no tiene privilegios para modificar el Beneficiario";
                    break;
                case 37:
                    titulos = "Alerta Usuario: ";
                    mensajes = "Usted no tiene privilegios para modificar el Propietario";
                    break;
                case 38:
                    titulos = "Alerta";
                    mensajes = "La información de este vehículo no puede ser ingresada por que se encuentra REPORTADO POR SEGURIDAD";
                    break;
                case 39:
                    titulos = "Alerta";
                    mensajes = "La información de esta persona no puede ser ingresada por que se encuentra REPORTADO POR SEGURIDAD";
                    break;
                case 40:
                    titulos = "Alerta";
                    mensajes = "La fecha hidrostátia está vencida";
                    break;
                case 41:
                    titulos = "Mensaje";
                    mensajes =  "Salvados Satisfactoriamente";
                    break;
                case 42:
                    titulos = "Mensaje";
                    mensajes = "La cuenta bancaria no coincide con el Id del conductor ni el nit del beneficiario";
                    break;
                case 43:
                    titulos = "Mensaje";
                    mensajes = "La información de esta hoja de vida no puede ser modificada por que el vehículo:";
                    break;
                case 44:
                    titulos = "Mensaje";
                    mensajes = "La información de esta hoja de vida no puede ser modificada por que el Conductor:";
                    break;
                case 45:
                    titulos = "Mensaje";
                    mensajes = "La información de esta hoja de vida no puede ser modificada por que el Beneficiario-Propietario:";
                    break;
                case 46:
                    titulos = "Alerta";
                    mensajes = "La fecha de la póliza de hidrocarburos está vencida";
                    break;
                case 47:
                    titulos = "Alerta";
                    mensajes = "La licencia de conducción está  vencida";
                    break;
                case 48:
                    titulos = "Alerta";
                    mensajes = "El número de celular no tiene la longitud permitida de 10 números para el: ";
                    break;
                case 49:
                    titulos = "Alerta";
                    mensajes = "El número de teléfono no tiene la longitud permitida de 7 números  para el: ";
                    break;
                case 50:
                    titulos = "Alerta";
                    mensajes = "Es obligatoria que exista una dirección registrada para el: ";
                    break;
                case 51:
                    titulos = "Alerta";
                    mensajes = "Es obligatoria que exista un teléfono fijo o celular registrado para el: ";
                    break;
                case 52:
                    titulos = "Alerta";
                    mensajes = "El número de teléfono no tiene la longitud permitida de 7 números o El número de celular no tiene la longitud permitida de 10 números para el: ";
                    break;
                case 53:
                    titulos = "Alerta";
                    mensajes = "la fecha de vencimiento de la licencia no corresponde a los parámetros del MINISTERIO DE TRANSPORTE";
                    break;
                case 54:
                    titulos = "Alerta";
                    mensajes = "No puede cargar un documento que no tenga asociado una placa o una identificación";
                    break;
                case 55:
                    titulos = "Alerta";
                    mensajes = "Esta placa no tiene conductor asignado";
                    break;
                case 56:
                    titulos = "Alerta";
                    mensajes = "Esta placa no tiene propietario asignado";
                    break;
                case 57:
                    titulos = "Alerta";
                    mensajes = "Esta placa no tiene beneficiario asignado";
                    break;
                case 58:
                    titulos = "Advertencia";
                    mensajes = "La fecha de vigencia de este documento no puede ser superior a 6  meses ";
                    break;
                case 59:
                    titulos = "Mensaje";
                    mensajes = " Mensajes enviados de manera exisota. ";
                    break;
                case 60:
                    titulos = "Mensaje";
                    mensajes = " A pesar de tener un reporte por seguridad, se permitirá editar los datos de esta placa PROPIA. ";
                    break;
                case 61:
                    titulos = "Alerta";
                    mensajes = "El contrato de vinculación esta vencido.";
                    break;
                case 62:
                    titulos = "Alerta";
                    mensajes = "El pago de la planilla de aporte de salud esta vencido.";
                    break;
                case 63:
                    titulos = "Alerta";
                    mensajes = "No tiene el contrato de vinculación.";
                    break;
                case 64:
                    titulos = "Alerta";
                    mensajes = "El documento ha sido eliminado de su ubicación, debe agregarlo de nuevo. (" + strComplemento + ")";
                    break;
                default:

                    break;
            }
        }


    }
}
