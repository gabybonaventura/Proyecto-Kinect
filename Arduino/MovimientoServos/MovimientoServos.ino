float Sensibilidad = 0.185; //sensibilidad en Voltios/Amperio para sensor de 5A

String incoming;
int CodoArribaAbajoPosicion;
int CodoIzquierdaDerechaPosicion;
int HombroArribaAbajoPosicion;
int HombroAdelanteAtrasPosicion;
String inString = "";    // string to hold input

long tiempoEnSegundos = 0;
long UltimoSegundo = 0;

byte pos = 0;

#include <Servo.h>

Servo CodoArribaAbajoServo;
Servo CodoIzquierdaDerechaServo;
Servo HombroArribaAbajoServo;
Servo HombroAdelanteAtrasServo;


void setup() {
  Serial.begin(9600);
  pinMode(13, OUTPUT);
  CodoArribaAbajoServo.attach(8);
  CodoIzquierdaDerechaServo.attach(9);
  HombroArribaAbajoServo.attach(10);
  HombroAdelanteAtrasServo.attach(11);

}

void loop() {
  tiempoEnSegundos = millis() / 1000; //La funcion millis() devuelve el tiempo en milisegundos desde que el arduino comenzo a ejecutar el sketch
  while (Serial.available() > 12) {
    inString = "";
    int inChar = Serial.read();
    if (inChar == '*')
    {
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      CodoArribaAbajoPosicion = inString.toInt();

      inString = "";
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      CodoIzquierdaDerechaPosicion = inString.toInt();

      inString = "";
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      HombroArribaAbajoPosicion = inString.toInt();

      inString = "";
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      HombroAdelanteAtrasPosicion = inString.toInt();

      Serial.print("Value:");
      Serial.println(CodoArribaAbajoPosicion);
      Serial.print("String: ");
      Serial.println(inString);
      Serial.print("Value:");
      Serial.println(CodoIzquierdaDerechaPosicion);
      Serial.print("String: ");
      Serial.println(inString);
      Serial.print("Value:");
      Serial.println(HombroArribaAbajoPosicion);
      Serial.print("String: ");
      Serial.println(inString);
      Serial.print("Value:");
      Serial.println(HombroAdelanteAtrasPosicion);
      Serial.print("String: ");
      Serial.println(inString);

      CodoArribaAbajoServo.write(CodoArribaAbajoPosicion);
      CodoIzquierdaDerechaServo.write(CodoIzquierdaDerechaPosicion);
      HombroArribaAbajoServo.write(HombroArribaAbajoPosicion);
      HombroAdelanteAtrasServo.write(HombroAdelanteAtrasPosicion);
    }
  }
    if(tiempoEnSegundos != UltimoSegundo)
    {
      //float voltajeSensor= analogRead(A0)*(5.0 / 1023.0); //lectura del sensor   
      //float I=(voltajeSensor-2.5)/Sensibilidad; //Ecuación  para obtener la corriente
      float I0=get_corriente(200, A0);//obtenemos la corriente promedio de 200 muestras 
      float I1=get_corriente(200, A1);//obtenemos la corriente promedio de 200 muestras 
      float I2=get_corriente(200, A2);//obtenemos la corriente promedio de 200 muestras 
      float I3=get_corriente(200, A3);//obtenemos la corriente promedio de 200 muestras 
      String aux = 'A' + String((int)abs(I0 * 1000));
      aux += 'B' + String((int)abs(I1 * 1000));
      aux += 'C' + String((int)abs(I2 * 1000));
      aux += 'D' + String((int)abs(I3 * 1000));
      Serial.println(aux);
      UltimoSegundo = tiempoEnSegundos;
    }
}

float get_corriente(int n_muestras, int pin)
{
  float voltajeSensor;
  float corriente=0;
  for(int i=0;i<n_muestras;i++)
  {
    voltajeSensor = analogRead(pin) * (5.0 / 1023.0);////lectura del sensor
    corriente=corriente+(voltajeSensor-2.5)/Sensibilidad; //Ecuación  para obtener la corriente
  }
  corriente=corriente/n_muestras;
  return(corriente);
}


