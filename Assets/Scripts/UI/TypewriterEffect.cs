using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TypewriterEffect : MonoBehaviour
    {
        private TMP_Text _textBox;

        private int _currentVisibleCharacterIndex;
        private Coroutine _typewriterCoroutine;
        private bool _readyForNewText = true;
        private WaitForSeconds _simpleDelay;
        private WaitForSeconds _interpunctuationDelay;

        [Header("Typewriter Settings")] 
        [SerializeField] private float charactersPerSecond = 20;
        [SerializeField] private float interpunctuationDelay = 0.5f;
        public bool CurrentlySkipping { get; private set; }
        private WaitForSeconds _skipDelay;

        [Header("Skip options")] 
        [SerializeField] private bool quickSkip;
        [SerializeField] [Min(1)] private int skipSpeedup = 5;

        private WaitForSeconds _textboxFullEventDelay;
        [SerializeField] [Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f; 

        public static event Action CompleteTextRevealed;
        public static event Action<char> CharacterRevealed;


        private void Awake()
        {
            _textBox = GetComponent<TMP_Text>();

            _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
            _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);
            _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
            _skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedup));

        }
        
        private void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
        }

        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1)
                    Skip(true);
            }
        }
        public bool IsTyping
        {
            get { return _readyForNewText == false; }
        }
       

         private void PrepareForNewText(Object obj)
         {
             if (obj != _textBox || !_readyForNewText){
                    return;
             }             
             _readyForNewText = false;
             CurrentlySkipping = false;

             if (_typewriterCoroutine != null){
                    StopCoroutine(_typewriterCoroutine);
                    _typewriterCoroutine = null;
             }
            
             _textBox.maxVisibleCharacters = 0;
             _currentVisibleCharacterIndex = 0;

             _typewriterCoroutine = StartCoroutine(Typewriter());
         }



        private IEnumerator Typewriter()
        {
            TMP_TextInfo textInfo = _textBox.textInfo;

            while (_currentVisibleCharacterIndex < textInfo.characterCount + 1)
            {
                var lastCharacterIndex = textInfo.characterCount - 1;

                if (_currentVisibleCharacterIndex >= lastCharacterIndex)
                {
                    _textBox.maxVisibleCharacters++;
                    yield return _textboxFullEventDelay;
                    CompleteTextRevealed?.Invoke();
                    _readyForNewText = true;
                    yield break;
                }

                char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

                _textBox.maxVisibleCharacters++;
                
                if (!CurrentlySkipping &&
                    (character == '?' || character == '.' || character == ',' || character == ':' ||
                     character == ';' || character == '!' || character == '-'))
                {
                    yield return _interpunctuationDelay;
                }
                else
                {
                    yield return CurrentlySkipping ? _skipDelay : _simpleDelay;
                }
                
                CharacterRevealed?.Invoke(character);
                _currentVisibleCharacterIndex++;
            }
        }
        private void Skip(bool quickSkipNeeded = false)
        {
            if (CurrentlySkipping)
                return;
            
            CurrentlySkipping = true;

            if (!quickSkip || !quickSkipNeeded)
            {
                StartCoroutine(SkipSpeedupReset());
                return;
            }

            StopCoroutine(_typewriterCoroutine);
            _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
            _readyForNewText = true;
            CompleteTextRevealed?.Invoke();
        }

        private IEnumerator SkipSpeedupReset()
        {
            yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount - 1);
            CurrentlySkipping = false;
        }
    }
}