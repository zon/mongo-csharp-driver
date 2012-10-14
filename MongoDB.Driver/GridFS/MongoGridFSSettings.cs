/* Copyright 2010-2012 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Driver.GridFS
{
    /// <summary>
    /// Represents setting for GridFS.
    /// </summary>
    [Serializable]
    public class MongoGridFSSettings : IEquatable<MongoGridFSSettings>
    {
        // private static fields
        private static MongoGridFSSettings __defaults = new MongoGridFSSettings();

        // private fields
        private int? _chunkSize;
        private string _root;
        private SafeMode _safeMode;
        private bool? _updateMD5;
        private bool? _verifyMD5;

        private bool _isFrozen;
        private int _frozenHashCode;

        // static constructor
        static MongoGridFSSettings()
        {
            __defaults = new MongoGridFSSettings
            {
                ChunkSize = 256 * 1024, // 256KiB
                Root = "fs",
                UpdateMD5 = true,
                VerifyMD5 = true
            };
            __defaults.Freeze();
        }

        // constructors
        /// <summary>
        /// Initializes a new instance of the MongoGridFSSettings class.
        /// </summary>
        public MongoGridFSSettings()
        {
        }

        // public static properties
        /// <summary>
        /// Gets or sets the default GridFS settings.
        /// </summary>
        public static MongoGridFSSettings Defaults
        {
            get { return __defaults; }
            set { __defaults = value; }
        }

        // public properties
        /// <summary>
        /// Gets or sets the chunk size.
        /// </summary>
        public int? ChunkSize
        {
            get { return _chunkSize; }
            set
            {
                if (_isFrozen) { ThrowFrozen(); }
                _chunkSize = value;
            }
        }

        /// <summary>
        /// Gets whether the settings are frozen.
        /// </summary>
        public bool IsFrozen
        {
            get { return _isFrozen; }
        }

        /// <summary>
        /// Gets or sets the root collection name (the files and chunks collection names are derived from the root).
        /// </summary>
        public string Root
        {
            get { return _root; }
            set
            {
                if (_isFrozen) { ThrowFrozen(); }
                _root = value;
            }
        }

        /// <summary>
        /// Gets or sets the safe mode.
        /// </summary>
        public SafeMode SafeMode
        {
            get { return _safeMode; }
            set
            {
                if (_isFrozen) { ThrowFrozen(); }
                _safeMode = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to udpate the MD5 hash on the server when a file is uploaded or modified.
        /// </summary>
        public bool? UpdateMD5
        {
            get { return _updateMD5; }
            set {
                if (_isFrozen) { ThrowFrozen(); }
                _updateMD5 = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to verify the MD5 hash when a file is uploaded or downloaded.
        /// </summary>
        public bool? VerifyMD5
        {
            get { return _verifyMD5; }
            set {
                if (_isFrozen) { ThrowFrozen(); }
                _verifyMD5 = value;
            }
        }

        // public operators
        /// <summary>
        /// Compares two MongoGridFSSettings.
        /// </summary>
        /// <param name="lhs">The first MongoGridFSSettings.</param>
        /// <param name="rhs">The other MongoGridFSSettings.</param>
        /// <returns>True if the two MongoGridFSSettings are not equal (or one is null and the other is not).</returns>
        public static bool operator !=(MongoGridFSSettings lhs, MongoGridFSSettings rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Compares two MongoGridFSSettings.
        /// </summary>
        /// <param name="lhs">The first MongoGridFSSettings.</param>
        /// <param name="rhs">The other MongoGridFSSettings.</param>
        /// <returns>True if the two MongoGridFSSettings are equal (or both null).</returns>
        public static bool operator ==(MongoGridFSSettings lhs, MongoGridFSSettings rhs)
        {
            return object.Equals(lhs, rhs);
        }

        // public methods
        /// <summary>
        /// Creates a clone of the settings.
        /// </summary>
        /// <returns>A clone of the settings.</returns>
        public MongoGridFSSettings Clone()
        {
            var clone = new MongoGridFSSettings();
            clone._chunkSize = _chunkSize;
            clone._root = _root;
            clone._safeMode = (_safeMode == null) ? null : _safeMode.Clone();
            clone._updateMD5 = _updateMD5;
            clone._verifyMD5 = _verifyMD5;
            return clone;
        }

        /// <summary>
        /// Compares this MongoGridFSSettings to another one.
        /// </summary>
        /// <param name="rhs">The other MongoGridFSSettings.</param>
        /// <returns>True if the two settings are equal.</returns>
        public bool Equals(MongoGridFSSettings rhs)
        {
            if (object.ReferenceEquals(rhs, null) || GetType() != rhs.GetType()) { return false; }
            return
                _chunkSize == rhs._chunkSize &&
                _root == rhs._root &&
                _safeMode == rhs._safeMode &&
                _updateMD5 == rhs._updateMD5 &&
                _verifyMD5 == rhs._verifyMD5;
        }

        /// <summary>
        /// Compares this MongoGridFSSettings to another object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the other objects is a MongoGridFSSettings and is equal to this one.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as MongoGridFSSettings); // works even if obj is null or of a different type
        }

        /// <summary>
        /// Freezes the settings.
        /// </summary>
        /// <returns>The frozen settings.</returns>
        public MongoGridFSSettings Freeze()
        {
            if (!_isFrozen)
            {
                if (_safeMode != null) { _safeMode = _safeMode.FrozenCopy(); }
                _frozenHashCode = GetHashCode();
                _isFrozen = true;
            }
            return this;
        }

        /// <summary>
        /// Returns a frozen copy of the settings.
        /// </summary>
        /// <returns>A frozen copy of the settings.</returns>
        public MongoGridFSSettings FrozenCopy()
        {
            if (_isFrozen)
            {
                return this;
            }
            else
            {
                return Clone().Freeze();
            }
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            if (_isFrozen)
            {
                return _frozenHashCode;
            }

            // see Effective Java by Joshua Bloch
            int hash = 17;
            hash = 37 * hash + ((_chunkSize == null) ? 0 : _chunkSize.GetHashCode());
            hash = 37 * hash + ((_root == null) ? 0 : _root.GetHashCode());
            hash = 37 * hash + ((_safeMode == null) ? 0 : _safeMode.GetHashCode());
            hash = 37 * hash + ((_updateMD5 == null) ? 0 : _updateMD5.GetHashCode());
            hash = 37 * hash + ((_verifyMD5 == null) ? 0 : _verifyMD5.GetHashCode());
            return hash;
        }

        // internal methods
        internal void ApplyInheritedSettings(MongoDatabaseSettings databaseSettings)
        {
            if (_chunkSize == null)
            {
                _chunkSize = __defaults.ChunkSize;
            }
            if (_root == null)
            {
                _root = __defaults.Root;
            }
            if (_safeMode == null)
            {
                _safeMode = databaseSettings.SafeMode;
            }
            if (_updateMD5 == null)
            {
                _updateMD5 = __defaults.UpdateMD5;
            }
            if (_verifyMD5 == null)
            {
                _verifyMD5 = __defaults.VerifyMD5;
            }
        }

        // private methods
        private void ThrowFrozen()
        {
            throw new InvalidOperationException("A MongoGridFSSettings object cannot be modified once it has been frozen.");
        }
    }
}
