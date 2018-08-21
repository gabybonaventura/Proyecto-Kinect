
String incoming;
int CodoPosicion;
int HombroArribaAbajoPosicion;
int HombroAdelanteAtrasPosicion;
String inString = "";    // string to hold input

long tiempoEnSegundos = 0;
long UltimoSegundo = 0;

byte pos = 0;

#include <Servo.h>

Servo CodoServo;
Servo HombroArribaAbajoServo;
Servo HombroAdelanteAtrasServo;


void setup(){
  Serial.begin(9600);
  pinMode(13, OUTPUT);
  CodoServo.attach(9);
  HombroArribaAbajoServo.attach(10);
  HombroAdelanteAtrasServo.attach(11);
  
}

void loop(){
  //tiempoEnSegundos=millis()/1000; //La funcion millis() devuelve el tiempo en milisegundos desde que el arduino comenzo a ejecutar el sketch
    while (Serial.available() > 9) {
    inString = "";
    int inChar = Serial.read();
    if(inChar == '*')
    {
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      inChar = Serial.read();
      inString += (char)inChar;
      CodoPosicion = inString.toInt();
      
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
      Serial.println(CodoPosicion);
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
      
      CodoServo.write(CodoPosicion);
      HombroArribaAbajoServo.write(HombroArribaAbajoPosicion);
      HombroAdelanteAtrasServo.write(HombroAdelanteAtrasPosicion);
    }
    /*
    // if you get a newline, print the string, then the string's value:
    if (inChar == '|') {
      Serial.print("Value:");
      Serial.println(inString.toInt());
      Serial.print("String: ");
      Serial.println(inString);
      // clear the string for new input:
      inString = "";
    }*/
  }
         /*
         HombroArribaAbajoPosicion = Serial.read();
         HombroAdelanteAtrasPosicion = Serial.read();
         Serial.write(incoming);
         Serial.print(CodoPosicion, DEC);
         Serial.print(HombroArribaAbajoPosicion, DEC);
         Serial.print(HombroAdelanteAtrasPosicion, DEC);

         Serial.print(pos, DEC);
         CodoServo.write(pos);
         HombroArribaAbajoServo.write(pos);
         HombroAdelanteAtrasServo.write(pos);
         */
        
      }
