using System;
using Microsoft.SPOT;
using GTM = Gadgeteer.Modules;
using GT = Gadgeteer;

namespace Practica3DSCC
{
    // Referencia tipo "delegate" para función callback ObjectOn
    public delegate void ObjectOnEventHandler();

    // Referencia tipo "delegate" para función callback ObjectOff
    public delegate void ObjectOffEventHandler();

    /*
     * Clase SensorProximidad, encapsula el funcionanmiento del sensor de proximidad infrarrojo.
     * Esta clase gestiona los dos componentes del sensor: el LED infrarrojo y el foto-transistor.
     * Además, dispara dos eventos: ObjectOn y ObjectOff cuando el sensor detecta la presencia o
     * ausencia de un objeto.
     */
    class SensorProximidad
    {
        //EVENTO ObjectOff: Disparar este evento cuando el sensor detecte la ausencia del objeto
        public event ObjectOffEventHandler ObjectOff;

        //EVENTO ObjectOn: Disparar este evento cuando el sensor detecte la presencia de un objeto
        public event ObjectOnEventHandler ObjectOn;

        GT.SocketInterfaces.DigitalOutput salida;
        
        public SensorProximidad(GTM.GHIElectronics.Extender extender)
        {
            salida = extender.CreateDigitalOutput(GT.Socket.Pin.Four,false);
            
            //TODO: Inicializar el sensor
        }

        public void StartSampling()
        {
            //TODO: Activar el LED infrarrojo y empezar a muestrear el foto-transistor
            salida.Write(true);
            Debug.Print("led encendido");
        }

        public void StopSampling()
        {
            salida.Write(false);
            Debug.Print("led apagado");
            //TODO: Desactivar el LED infrarrojo y detener el muestreo del foto-transistor
        }
    }
}
