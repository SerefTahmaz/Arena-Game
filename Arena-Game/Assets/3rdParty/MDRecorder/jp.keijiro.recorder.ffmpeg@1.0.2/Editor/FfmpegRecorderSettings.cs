using UnityEngine;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using System.Collections.Generic;

namespace FFmpegOut.Recorder
{
    [RecorderSettings(typeof(FfmpegRecorder), "MD Recorder", "movie_16")]
    sealed class FfmpegRecorderSettings : RecorderSettings
    {
        public FFmpegPreset preset = FFmpegPreset.H264Default;
        public bool flipImage = false;

        [SerializeField] ImageInputSelector _imageInputSelector = new ImageInputSelector();

        public FfmpegRecorderSettings()
        {
            FileNameGenerator.FileName = "XXX_uncut_<Take>";
            _imageInputSelector.ForceEvenResolutionPublic(true);
        }

        public bool FlipImageArg {
            get {
                return flipImage;
            }
        }

        public override IEnumerable<RecorderInputSettings> InputsSettings
        {
            get { yield return _imageInputSelector.Selected; }
        } 

        protected override string Extension {
            get { return preset.GetSuffix().Substring(1); }
        }
    }
}
