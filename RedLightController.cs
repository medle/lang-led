using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Solid.Arduino;
using Solid.Arduino.Firmata;

namespace LangLed
{
  public class RedLightController : IDisposable
  {
    private const int PWM_PIN = 11;
    private const int LED_PIN = 13;
    private ISerialConnection connection;
    private IFirmataProtocol session;
    private string stateText = "initial";

    public RedLightController()
    {
    }

    public void TurnLight(bool on)
    {
      try {
        DoTurnLight(on);
      }
      catch (Exception e) {
        SetState(e.Message);
        Dispose();
      }
    }

    private void DoTurnLight(bool on)
    {
      if (this.session == null) {
        this.connection = EnhancedSerialConnection.Find();
        if (this.connection != null) {
          this.session = new ArduinoSession(this.connection);
          session.SetDigitalPinMode(PWM_PIN, PinMode.PwmOutput);
          session.SetDigitalPinMode(LED_PIN, PinMode.DigitalOutput);
          SetState($"Firmata on {connection.PortName} (PWM pin {PWM_PIN})");
        }
      }

      if (this.session != null) {
        int pwmWidthByte = ((LangLedMain.BrightnessPercentage * 255) / 100);
        session.SetDigitalPin(PWM_PIN, on ? pwmWidthByte : 0);
        session.SetDigitalPin(LED_PIN, on);
      }
    }

    public void Dispose()
    {
      if (this.connection != null) {
        this.connection.Dispose();
        this.connection = null;
        this.session = null;
        SetState("disconnected");
      }
    }

    private void SetState(string text)
    {
      this.stateText = text;
      //System.Diagnostics.Trace.WriteLine(text);
    }

    public string GetState()
    {
      return this.stateText;
    }
  }
}
