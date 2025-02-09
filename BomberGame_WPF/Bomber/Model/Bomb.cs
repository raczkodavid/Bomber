using System;
using System.Timers;
namespace Bomber.Model;

public class Bomb : IDisposable
{
    #region Public Properties

    /// <summary>
    /// Gets the position of the bomb.
    /// </summary>
    public Position Position { get; }

    #endregion

    #region Private Fields

    /// <summary>
    /// The radius of the bomb's explosion.
    /// </summary>
    private readonly int _explosionRadius;

    /// <summary>
    /// Timer that triggers the bomb explosion after a set time (in milliseconds).
    /// </summary>
    private readonly Timer _timer;

    #endregion

    #region Events

    /// <summary>
    /// Occurs when the bomb explodes.
    /// </summary>
    public event EventHandler<BombExplodedEventArgs>? BombExploded;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Bomb"/> class.
    /// </summary>
    /// <param name="position">The position of the bomb.</param>
    /// <param name="explosionRadius">The radius of the explosion. Default is 3.</param>
    public Bomb(Position position, int explosionRadius = 3)
    {
        Position = position;
        _explosionRadius = explosionRadius;
        _timer = new Timer(3000); // 3 seconds
        _timer.Elapsed += (_, _) => OnBombExploded();
        _timer.Start();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Pauses the bomb's countdown timer.
    /// </summary>
    public void PauseCountdown() => _timer.Stop();

    /// <summary>
    /// Resumes the bomb's countdown timer.
    /// </summary>
    public void ResumeCountdown() => _timer.Start();

    /// <summary>
    /// Function to release resources used by the bomb.
    /// </summary>
    public void Dispose() => _timer.Dispose();

    #endregion

    #region Event Triggers

    /// <summary>
    /// Raises the <see cref="BombExploded"/> event.
    /// </summary>
    private void OnBombExploded()
    {
        BombExploded?.Invoke(this, new BombExplodedEventArgs(Position, _explosionRadius));
        _timer.Stop();
        _timer.Dispose();
    }

    #endregion
}