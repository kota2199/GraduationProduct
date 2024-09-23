using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace neoludicGames.uDmx
{
    [ExecuteAlways]
    public class DmxController : MonoBehaviour
    {
        /// <summary>
        /// Returns the state of the DMX sender. Note that it returns false, both if there is no DMX Controller in the scene, and if there is a problem with the networked connection.
        /// </summary>
        public static bool isDmxSenderFunctional => _instance ? _instance.dmxSenderIsFunctional : false;
        
        /// <summary>
        /// Event being invoked if the connection to the dmx interface, or something with the dmx interface itself changes.
        /// </summary>
        public static UnityEvent<bool> dmxStateChanged;
        
        [Tooltip("This is the URI address of the of the USB-DMX Server. It can be on this computer, as in the case of localhost, it can reference a device on your local network, or even a remote device.")]
        [SerializeField] private string uri = "http://localhost:14444";
        [SerializeField] private bool updateLightsDuringEditMode = true;
        [SerializeField] private UnityEvent dmxFunctioning, dmxNotFunctioning;

        private static byte[] _lightData = new Byte[513];
        private static bool _readyToSend = true;
        private static bool _dirty = true;
        private static DmxController _instance;
        private YieldInstruction _sendingDelay = null;
        private bool dmxSenderIsFunctional = false;

        private UnityWebRequest _webRequest = null;
        
        public void SetLightServerURL(string url) => uri = url;
        public void SetLightServerURL(string url, int port) => uri = string.Format("{0}:{1}", url, port);
        
        /// <summary>
        /// DMX style start value (1-512) and a variable list of attributes for the subsequent channels.
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="values"></param>
        public static void SetLightData(int startValue, params byte[] values)
        {
            startValue--;
            if (_lightData == null || _lightData.Length < 513) _lightData = new byte[513];
            for (int i = startValue; i < startValue + values.Length && i < _lightData.Length; i++)
                _lightData[i] = values[i - startValue];
            _dirty = true;
            if (_readyToSend)
            {
                _instance ??= FindObjectOfType<DmxController>();
                _instance?.SendValuesUnityWebRequest();
            }
        }

        [ExecuteAlways]
        void OnEnable()
        {
            _instance = this;
            _lightData = new byte[513];
            dmxNotFunctioning?.Invoke();
        }

        private void SetDMXState(bool state, string message = "")
        {
            if (state != dmxSenderIsFunctional)
            {
                dmxStateChanged?.Invoke(state);
                if (state) dmxFunctioning?.Invoke();
                else dmxNotFunctioning?.Invoke();
            }
            dmxSenderIsFunctional = state;
        }

        private void SendValuesUnityWebRequest()
        {
            _dirty = false;
            _readyToSend = false;
            _webRequest = UnityWebRequest.Put(uri, _lightData);
            _webRequest.disposeCertificateHandlerOnDispose = true;
            _webRequest.disposeDownloadHandlerOnDispose = true;
            _webRequest.disposeUploadHandlerOnDispose = true;
            _webRequest.SendWebRequest().completed += x =>
            {
                if (_webRequest == null)
                {
                    _readyToSend = true;
                    return;
                }
                if (_webRequest.result != UnityWebRequest.Result.Success) SetDMXState(false, "Network Error");
                else if (_webRequest.downloadHandler.data == null || _webRequest.downloadHandler.data.Length == 0 ||
                         _webRequest.downloadHandler.data[0] == byte.MinValue) SetDMXState(false, "USB Error");
                else SetDMXState(true);
                
                _webRequest.Dispose();
                _webRequest = null;
                
                if (_dirty) SendValuesUnityWebRequest();
                else _readyToSend = true;
            };
        }
    }
}