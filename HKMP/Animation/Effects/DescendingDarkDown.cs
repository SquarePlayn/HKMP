﻿using UnityEngine;

namespace Hkmp.Animation.Effects {
    /// <summary>
    /// Animation effect class for the movement of going down from a Descending Dark.
    /// </summary>
    internal class DescendingDarkDown : QuakeDownBase {
        /// <inheritdoc/>
        public override void Play(GameObject playerObject, bool[] effectInfo) {
            // Call the play method with the correct Q Trail prefab name
            Play(playerObject, effectInfo, "Q Trail 2");
        }
    }
}