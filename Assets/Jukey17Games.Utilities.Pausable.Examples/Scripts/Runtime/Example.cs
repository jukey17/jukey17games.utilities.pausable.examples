using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jukey17Games.Utilities.Pausable.Examples.Runtime
{
    internal sealed class Example : MonoBehaviour
    {
        private enum Groups
        {
            Animator = 1 << 0,
            RigidBody = 1 << 1,
            Particle = 1 << 2,
            Spawn = 1 << 3,
        }

        [SerializeField] private GameObject cubePrefab = default;
        [SerializeField] private Transform spawnPoint = default;
        [SerializeField] private ParticleSystemPauseSwitcher particlePauseSwitcher = default;
        [SerializeField] private Button switchPauseSystemButton = default;
        [SerializeField] private TextMeshProUGUI switchPauseSystemText = default;
        [SerializeField] private Button switchPauseAnimatorButton = default;
        [SerializeField] private TextMeshProUGUI switchPauseAnimatorText = default;
        [SerializeField] private Button switchPauseRigidbodyButton = default;
        [SerializeField] private TextMeshProUGUI switchPauseRigidbodyText = default;
        [SerializeField] private Button switchPauseParticleButton = default;
        [SerializeField] private TextMeshProUGUI switchPauseParticleText = default;
        [SerializeField] private Button switchPauseSpawnButton = default;
        [SerializeField] private TextMeshProUGUI switchPauseSpawnText = default;

        private readonly IGroupPausableSystem _pausableSystem = new GroupPausableSystem();
        private readonly IPausableTimer _spawnTimer = new PausableTimer(false, 1000);

        private bool _isAnimatorPause;
        private bool _isRigidbodyPause;
        private bool _isParticlePause;
        private bool _isSpawnPause;

        private void Awake()
        {
            _pausableSystem.Register(particlePauseSwitcher, (int) Groups.Particle);

            _spawnTimer.AddOnElapsed(_ => SpawnCube());
            _pausableSystem.Register(_spawnTimer, (int) Groups.Spawn);

            switchPauseSystemButton.onClick.AddListener(SwitchPauseSystem);
            switchPauseAnimatorButton.onClick.AddListener(SwitchPauseAnimator);
            switchPauseRigidbodyButton.onClick.AddListener(SwitchPauseRigidbody);
            switchPauseParticleButton.onClick.AddListener(SwitchPauseParticle);
            switchPauseSpawnButton.onClick.AddListener(SwitchPauseSpawn);
        }

        private void Start()
        {
            _spawnTimer.Start();
        }

        private void OnDestroy()
        {
            _spawnTimer.Dispose();
        }

        private void SpawnCube()
        {
            var go = Instantiate(cubePrefab, spawnPoint);
            var animatorPauseSwitcher = go.GetComponent<IAnimatorPauseSwitcher>();
            _pausableSystem.Register(animatorPauseSwitcher, (int) Groups.Animator);
            var rigidbodyPauseSwitcher = go.GetComponent<IRigidbodyPauseSwitcher>();
            _pausableSystem.Register(rigidbodyPauseSwitcher, (int) Groups.RigidBody);
        }

        private void SwitchPauseSystem()
        {
            if (_pausableSystem.IsPausing)
            {
                _pausableSystem.Resume();
                switchPauseSystemText.text = "Pause System";
                switchPauseAnimatorText.text = "Pause Animator";
                switchPauseRigidbodyText.text = "Pause Rigidbody";
                switchPauseParticleText.text = "Pause Particle";
                switchPauseSpawnText.text = "Pause Spawn";
                _isAnimatorPause = false;
                _isRigidbodyPause = false;
                _isParticlePause = false;
                _isSpawnPause = false;
            }
            else
            {
                _pausableSystem.Pause();
                switchPauseSystemText.text = "Resume System";
                switchPauseAnimatorText.text = "Resume Animator";
                switchPauseRigidbodyText.text = "Resume Rigidbody";
                switchPauseParticleText.text = "Resume Particle";
                switchPauseSpawnText.text = "Resume Spawn";
                _isAnimatorPause = true;
                _isRigidbodyPause = true;
                _isParticlePause = true;
                _isSpawnPause = true;
            }
        }

        private void SwitchPauseAnimator()
        {
            if (_isAnimatorPause)
            {
                _pausableSystem.Resume((int) Groups.Animator);
                switchPauseAnimatorText.text = "Pause Animator";
                _isAnimatorPause = false;
            }
            else
            {
                _pausableSystem.Pause((int) Groups.Animator);
                switchPauseAnimatorText.text = "Resume Animator";
                _isAnimatorPause = true;
            }
        }

        private void SwitchPauseRigidbody()
        {
            if (_isRigidbodyPause)
            {
                _pausableSystem.Resume((int) Groups.RigidBody);
                switchPauseRigidbodyText.text = "Pause Rigidbody";
                _isRigidbodyPause = false;
            }
            else
            {
                _pausableSystem.Pause((int) Groups.RigidBody);
                switchPauseRigidbodyText.text = "Resume Rigidbody";
                _isRigidbodyPause = true;
            }
        }

        private void SwitchPauseParticle()
        {
            if (_isParticlePause)
            {
                _pausableSystem.Resume((int) Groups.Particle);
                switchPauseParticleText.text = "Pause Particle";
                _isParticlePause = false;
            }
            else
            {
                _pausableSystem.Pause((int) Groups.Particle);
                switchPauseParticleText.text = "Resume Particle";
                _isParticlePause = true;
            }
        }

        private void SwitchPauseSpawn()
        {
            if (_isSpawnPause)
            {
                _pausableSystem.Resume((int) Groups.Spawn);
                switchPauseSpawnText.text = "Pause Spawn";
                _isSpawnPause = false;
            }
            else
            {
                _pausableSystem.Pause((int) Groups.Spawn);
                switchPauseSpawnText.text = "Resume Spawn";
                _isSpawnPause = true;
            }
        }
    }
}
