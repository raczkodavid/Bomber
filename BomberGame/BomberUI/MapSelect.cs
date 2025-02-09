using System;
using System.Windows.Forms;

namespace BomberUI
{
    /// <summary>
    /// Represents the form for selecting maps before starting the game.
    /// </summary>
    public partial class MapSelect : Form
    {
        #region Private Fields

        /// <summary>
        /// Holds a reference to the GameForm.
        /// </summary>
        private GameForm? _gameForm;

        /// <summary>
        /// Indicates if the form is closing.
        /// </summary>
        public bool IsClosing { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MapSelect class.
        /// </summary>
        public MapSelect()
        {
            InitializeComponent();

            // Set up the event handlers for the picture boxes
            smallMapPictureBox.Click += OnPictureBoxClicked;
            mediumMapPictureBox.Click += OnPictureBoxClicked;
            largeMapPictureBox.Click += OnPictureBoxClicked;
            customMapPictureBox.Click += OnPictureBoxClicked;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the click event for the picture boxes to select maps.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnPictureBoxClicked(object? sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender!;
            string mapPath = String.Empty;

            // Assign map paths based on which picture box was clicked
            switch (pictureBox.Name)
            {
                case "smallMapPictureBox":
                    mapPath = "Maps\\map10x10.txt";
                    break;
                case "mediumMapPictureBox":
                    mapPath = "Maps\\map15x15.txt";
                    break;
                case "largeMapPictureBox":
                    mapPath = "Maps\\map20x20.txt";
                    break;
                case "customMapPictureBox":
                    // Let the user select a custom map
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Filter = "Text files (*.txt)|*.txt";
                        openFileDialog.Title = "Select a custom map";

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            mapPath = openFileDialog.FileName;
                        }
                        else
                        {
                            return; // Exit if no file is selected
                        }
                    }
                    break;
            }

            // Proceed if a valid map path has been determined
            if (!string.IsNullOrEmpty(mapPath))
            {
                // Check if GameForm is already created
                if (_gameForm == null || _gameForm.IsDisposed)
                {
                    // Create a new GameForm if it's not already created or has been disposed
                    _gameForm = new GameForm(this);

                    // Check if map loading was succesful, if not just return so the user can select a valid map
                    if (!_gameForm.LoadMap(mapPath))
                    {
                        MessageBox.Show("Please Select a valid map!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _gameForm.Show();
                }
                else
                {
                    // Check if map loading was succesful, if not just return so the user can select a valid map
                    if (!_gameForm.LoadMap(mapPath))
                    {
                        MessageBox.Show("Please Select a valid map!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _gameForm.Show();
                }

                // Hide MapSelect after a map is selected
                Hide();
            }
            else
            {
                MessageBox.Show("No map was selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Form LifeCycle

        /// <summary>
        /// Handles the form closed event to clean up resources.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IsClosing = true;
            base.OnFormClosed(e); // Call base method first to ensure proper form cleanup

            // Close the GameForm if it exists and hasn't been disposed
            if (_gameForm != null && !_gameForm.IsDisposed && !_gameForm.IsClosing)
                _gameForm.Close();
        }

        #endregion
    }
}
