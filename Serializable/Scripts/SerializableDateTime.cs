using System;
using UnityEngine;

namespace SimTheory {
    [Serializable]
    public class SerializableDateTime {

        [Range(1, 9999)]
        public int year = 1900;
        [Range(1, 12)]
        public int month = 1;
        [Range(1, 31)]
        public int day = 1;
        [Range(0, 23)]
        public int hour = 0;
        [Range(0, 59)]
        public int minute = 0;
        [Range(0, 59)]
        public int second = 0;



        public DateTime Value {
            get {
                try {
                    return new DateTime(year, month, day, hour, minute, second);
                }
                catch {
                    // Return a fallback value if the date is invalid
                    return DateTime.MinValue;
                }
            }
            set {
                year = value.Year;
                month = value.Month;
                day = value.Day;
                hour = value.Hour;
                minute = value.Minute;
                second = value.Second;
            }
        }
        public int Year => Value.Year;
        public int Month => Value.Month;
        public int Day => Value.Day;
        public int Hour => Value.Hour;
        public int Minute => Value.Minute;
        public int Second => Value.Second;
        public DayOfWeek DayOfWeek => Value.DayOfWeek;
        public int DayOfYear => Value.DayOfYear;
        public int Millisecond => Value.Millisecond;
        public long Ticks => Value.Ticks;
        public TimeSpan TimeOfDay => Value.TimeOfDay;

        public SerializableDateTime() { }
        public SerializableDateTime(DateTime dateTime) {
            Value = dateTime;
        }
        public SerializableDateTime(int year, int month, int day, int hour, int minute, int second) {
            this.year = Mathf.Clamp(year, 1, 9999);
            this.month = Mathf.Clamp(month, 1, 12);
            this.day = Mathf.Clamp(day, 1, 31);
            this.hour = Mathf.Clamp(hour, 0, 23);
            this.minute = Mathf.Clamp(minute, 0, 59);
            this.second = Mathf.Clamp(second, 0, 59);
        }

        public SerializableDateTime AddSeconds(double seconds) {
            Value = Value.AddSeconds(seconds);
            return this;
        }
        public override string ToString() {
            return Value.ToString("MM-dd-yyyy HH:mm:ss");
        }



        public static bool operator ==(SerializableDateTime a, SerializableDateTime b) {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Value == b.Value;
        }
        public static bool operator !=(SerializableDateTime a, SerializableDateTime b) {
            return !(a == b);
        }
        public static bool operator <(SerializableDateTime a, SerializableDateTime b) {
            if (a is null || b is null) throw new ArgumentNullException();
            return a.Value < b.Value;
        }
        public static bool operator >(SerializableDateTime a, SerializableDateTime b) {
            if (a is null || b is null) throw new ArgumentNullException();
            return a.Value > b.Value;
        }
        public static bool operator <=(SerializableDateTime a, SerializableDateTime b) {
            if (a is null || b is null) throw new ArgumentNullException();
            return a.Value <= b.Value;
        }
        public static bool operator >=(SerializableDateTime a, SerializableDateTime b) {
            if (a is null || b is null) throw new ArgumentNullException();
            return a.Value >= b.Value;
        }
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is SerializableDateTime other)
                return this.Value == other.Value;
            return false;
        }
        public override int GetHashCode() {
            return Value.GetHashCode();
        }
        public static implicit operator DateTime(SerializableDateTime sdt) {
            return sdt?.Value ?? DateTime.MinValue;
        }
        public static implicit operator SerializableDateTime(DateTime dt) {
            var sdt = new SerializableDateTime();
            sdt.Value = dt;
            return sdt;
        }
    }
}