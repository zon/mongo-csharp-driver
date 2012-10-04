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

using MongoDB.Bson;

namespace MongoDB.Driver
{
    /// <summary>
    /// The settings used to access a collection.
    /// </summary>
    public class MongoCollectionSettings
    {
        // private fields
        private bool? _assignIdOnInsert;
        private GuidRepresentation? _guidRepresentation;
        private ReadPreference _readPreference;
        private SafeMode _safeMode;

        // the following fields are set when Freeze is called
        private bool _isFrozen;
        private int _frozenHashCode;
        private string _frozenStringRepresentation;

        // constructors
        /// <summary>
        /// Initializes a new instance of the MongoCollectionSettings class.
        /// </summary>
        public MongoCollectionSettings()
        {
        }

        // public properties
        /// <summary>
        /// Gets or sets whether the driver should assign Id values when missing.
        /// </summary>
        public bool? AssignIdOnInsert
        {
            get { return _assignIdOnInsert; }
            set
            {
                if (_isFrozen) { throw new InvalidOperationException("MongoCollectionSettings is frozen."); }
                _assignIdOnInsert = value;
            }
        }

        /// <summary>
        /// Gets or sets the representation used for Guids.
        /// </summary>
        public GuidRepresentation? GuidRepresentation
        {
            get { return _guidRepresentation; }
            set
            {
                if (_isFrozen) { throw new InvalidOperationException("MongoCollectionSettings is frozen."); }
                _guidRepresentation = value;
            }
        }

        /// <summary>
        /// Gets whether the settings have been frozen to prevent further changes.
        /// </summary>
        public bool IsFrozen
        {
            get { return _isFrozen; }
        }

        /// <summary>
        /// Gets or sets the read preference to use.
        /// </summary>
        public ReadPreference ReadPreference
        {
            get { return _readPreference; }
            set
            {
                if (_isFrozen) { throw new InvalidOperationException("MongoCollectionSettings is frozen."); }
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _readPreference = value;
            }
        }

        /// <summary>
        /// Gets or sets the SafeMode to use.
        /// </summary>
        public SafeMode SafeMode
        {
            get { return _safeMode; }
            set
            {
                if (_isFrozen) { throw new InvalidOperationException("MongoCollectionSettings is frozen."); }
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _safeMode = value;
            }
        }

        // public methods
        /// <summary>
        /// Creates a clone of the settings.
        /// </summary>
        /// <returns>A clone of the settings.</returns>
        public MongoCollectionSettings Clone()
        {
            var clone = new MongoCollectionSettings();
            clone._assignIdOnInsert = _assignIdOnInsert;
            clone._guidRepresentation = _guidRepresentation;
            clone._readPreference = _readPreference;
            clone._safeMode = _safeMode;
            return clone;
        }

        /// <summary>
        /// Compares two MongoCollectionSettings instances.
        /// </summary>
        /// <param name="obj">The other instance.</param>
        /// <returns>True if the two instances are equal.</returns>
        public override bool Equals(object obj)
        {
            var rhs = obj as MongoCollectionSettings;
            if (rhs == null)
            {
                return false;
            }
            else
            {
                if (_isFrozen && rhs._isFrozen)
                {
                    return _frozenStringRepresentation == rhs._frozenStringRepresentation;
                }
                else
                {
                    return
                        _assignIdOnInsert == rhs._assignIdOnInsert &&
                        _guidRepresentation == rhs._guidRepresentation &&
                        _readPreference == rhs._readPreference &&
                        _safeMode == rhs._safeMode;
                }
            }
        }

        /// <summary>
        /// Freezes the settings.
        /// </summary>
        /// <returns>The frozen settings.</returns>
        public MongoCollectionSettings Freeze()
        {
            if (!_isFrozen)
            {
                _readPreference = (_readPreference == null) ? null : _readPreference.FrozenCopy();
                _safeMode = (_safeMode == null) ? null : _safeMode.FrozenCopy();
                _frozenHashCode = GetHashCode();
                _frozenStringRepresentation = ToString();
                _isFrozen = true;
            }
            return this;
        }

        /// <summary>
        /// Returns a frozen copy of the settings.
        /// </summary>
        /// <returns>A frozen copy of the settings.</returns>
        public MongoCollectionSettings FrozenCopy()
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
            hash = 37 * hash + ((_assignIdOnInsert == null) ? 0 : _assignIdOnInsert.GetHashCode());
            hash = 37 * hash + ((_guidRepresentation == null) ? 0 : _guidRepresentation.GetHashCode());
            hash = 37 * hash + ((_readPreference == null) ? 0 : _readPreference.GetHashCode());
            hash = 37 * hash + ((_safeMode == null) ? 0 :_safeMode.GetHashCode());
            return hash;
        }

        /// <summary>
        /// Returns a string representation of the settings.
        /// </summary>
        /// <returns>A string representation of the settings.</returns>
        public override string ToString()
        {
            if (_isFrozen)
            {
                return _frozenStringRepresentation;
            }

            return string.Format(
                "AssignIdOnInsert={0};GuidRepresentation={1};ReadPreference={2};SafeMode={3}",
                _assignIdOnInsert, _guidRepresentation, _readPreference, _safeMode);
        }

        // internal methods
        internal MongoCollectionSettings ApplyInheritedSettings(MongoDatabaseSettings databaseSettings)
        {
            var clone = Clone();

            if (_assignIdOnInsert == null)
            {
                clone._assignIdOnInsert = MongoDefaults.AssignIdOnInsert;
            }
            if (_guidRepresentation == null)
            {
                clone._guidRepresentation = databaseSettings.GuidRepresentation;
            }
            if (_readPreference == null)
            {
                clone._readPreference = databaseSettings.ReadPreference;
            }
            if (_safeMode == null)
            {
                clone._safeMode = databaseSettings.SafeMode;
            }

            clone.Freeze(); // return already frozen
            return clone;
        }
    }
}
