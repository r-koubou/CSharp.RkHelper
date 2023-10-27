using System;

using RkHelper.Primitives;

namespace RkHelper.Testing.Primitives
{
    using NUnit.Framework;

    [TestFixture]
    public class ArrayHelperTests
    {
        [Test]
        public void ValidateArrayRange_ValidRange_DoesNotThrow()
        {
            int[] array = { 1, 2, 3, 4, 5 };
            Assert.DoesNotThrow(() => ArrayHelper.ValidateArrayRange(array, 1, 3));
        }

        [Test]
        public void ValidateArrayRange_ArrayIsNull_ThrowsArgumentNullException()
        {
            int[] array = null;
            Assert.Throws<ArgumentNullException>(() => ArrayHelper.ValidateArrayRange(array, 0, 0));
        }

        [Test]
        public void ValidateArrayRange_OffsetIsNegative_ThrowsArgumentOutOfRangeException()
        {
            int[] array = { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>(() => ArrayHelper.ValidateArrayRange(array, -1, 2));
        }

        [Test]
        public void ValidateArrayRange_LengthIsNegative_ThrowsArgumentOutOfRangeException()
        {
            int[] array = { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>(() => ArrayHelper.ValidateArrayRange(array, 0, -1));
        }

        [Test]
        public void ValidateArrayRange_OffsetPlusLengthExceedsArrayLength_ThrowsArgumentException()
        {
            int[] array = { 1, 2, 3 };
            Assert.Throws<ArgumentException>(() => ArrayHelper.ValidateArrayRange(array, 1, 3));
        }

        [Test]
        public void ValidateArrayRange_OffsetExceedsArrayLength_ThrowsArgumentException()
        {
            int[] array = { 1, 2, 3 };
            Assert.Throws<ArgumentException>(() => ArrayHelper.ValidateArrayRange(array, 4, 0));
        }
    }

}
