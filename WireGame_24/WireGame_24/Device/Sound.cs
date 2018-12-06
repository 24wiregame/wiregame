using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;



namespace WireGame_24.Device
{
    class Sound
    {
        #region
        private ContentManager contentManager;
        private Dictionary<string, Song> bgms;
        private Dictionary<string, SoundEffect> soundEffects;
        private Dictionary<string, SoundEffectInstance> seInstances;
        private Dictionary<string, SoundEffectInstance> sePlayDict;
        private string currentBGM;

        public Sound(ContentManager content)
        {
            contentManager = content;
            MediaPlayer.IsRepeating = true;

            bgms = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            seInstances = new Dictionary<string, SoundEffectInstance>();
            sePlayDict = new Dictionary<string, SoundEffectInstance>();
            currentBGM = null;
        }
        public void Unload()
        {
            bgms.Clear();
            soundEffects.Clear();
            seInstances.Clear();
            sePlayDict.Clear();
        }
        #endregion
        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名(" + name + ")がありません" + "アセット名の確認Dictionalyに登録しているか確認してください";

        }
        #region BGM(MP3:MediaPlayer)

        public void LoadBGM(string name, string filepath = "./")
        {
            if (bgms.ContainsKey(name))
            {
                return;
            }
            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }
        public bool IsStoppedBGM()
        {
            return (MediaPlayer.State == MediaState.Stopped);
        }
        public bool IsPlayingBGM()
        {
            return (MediaPlayer.State == MediaState.Playing);
        }
        public bool IsPauseBGM()
        {
            return (MediaPlayer.State == MediaState.Paused);
        }
        public void StopBGM()
        {
            MediaPlayer.Stop();
            currentBGM = null;
        }
        public void PlayBGM(string name)
        {
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));

            if (currentBGM == name)
            {
                return;
            }
            if (IsPlayingBGM())
            {
                StopBGM();
            }
            MediaPlayer.Volume = 0.5f;
            currentBGM = name;
            MediaPlayer.Play(bgms[currentBGM]);
        }
        public void PauseBGM()
        {
            if (IsPlayingBGM())
            {
                MediaPlayer.Pause();
            }
        }
        public void ResumeBGM()
        {
            if (IsPauseBGM())
            {
                MediaPlayer.Resume();
            }
        }
        public void ChangeBGMLoopFlag(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }
        #endregion BGM(MP3:MediaPlayer)
        #region WAV(SE:SoundEfffect)
        public void LoadSE(string name, string filepath = "./")
        {
            if (soundEffects.ContainsKey(name))
            {
                return;
            }
            soundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }
        public void PlaySE(string name)
        {
            Debug.Assert(soundEffects.ContainsKey(name), ErrorMessage(name));
            soundEffects[name].Play();
        }
        #endregion
        #region

        public void CreateSEInstance(string name)
        {
            if (seInstances.ContainsKey(name))
            {
                return;
            }
            Debug.Assert(
                soundEffects.ContainsKey(name),
                "先に" + name + "の読み込みを行ってください");
            seInstances.Add(name, soundEffects[name].CreateInstance());
        }
        public void PlaySEInstances(string name, int no, bool loopFLag = false)
        {
            Debug.Assert(
                seInstances.ContainsKey(name), ErrorMessage(name));
            if (sePlayDict.ContainsKey(name + no))
            {
                return;
            }
            var data = seInstances[name];
            data.IsLooped = loopFLag;
            data.Play();
            sePlayDict.Add(name + no, data);
        }
        public void StoppedSE(string name, int no)
        {
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }
            if (sePlayDict[name + no].State == SoundState.Playing)
            {
                sePlayDict[name + no].Stop();
            }
        }
        public void StoppedSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Playing)
                {
                    se.Value.Stop();
                }
            }
        }
        public void RemoveSE(string name, int no)
        {
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }
            sePlayDict.Remove(name + no);
        }
        public void RemoveSE()
        {
            sePlayDict.Clear();
        }
        public void PauseSE(string name, int no)
        {
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }
            if (sePlayDict[name + no].State == SoundState.Playing)
            {
                sePlayDict[name + no].Pause();
            }
        }
        public void PauseSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Playing)
                {
                    se.Value.Pause();
                }
            }
        }
        public void ResumeSE(string name, int no)
        {
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }
            if (sePlayDict[name + no].State == SoundState.Paused)
            {
                sePlayDict[name + no].Resume();
            }
        }
        public void ResumeSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Paused)
                {
                    se.Value.Resume();
                }
            }
        }
        public bool IsPlayingSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Playing;
        }
        public bool IsStoppedSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Stopped;
        }
        public bool IsPausedSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Paused;
        }

        #endregion

    }
}
