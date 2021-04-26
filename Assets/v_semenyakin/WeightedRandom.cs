using UnityEngine;

public static class WeightedRandom
{
    public struct Option<OptionType>
    {
        public Option(int inWeight, OptionType inValue)
        {
            _weight = inWeight;
            _value = inValue;
        }

        public int _weight;
        public OptionType _value;
    }

    public class WeightedRandomStream<OptionType>
    {
        public WeightedRandomStream(Option<OptionType>[] inOptions) {
            _options = inOptions;

            initCache();
        }

        private void initCache() {
            foreach (Option<OptionType> option in _options)
                _sumOfWeights += option._weight;
        }

        public OptionType getNext() {
            int random = Random.Range(0, _sumOfWeights);

            foreach (Option<OptionType> option in _options) {
                if (random < option._weight)
                    return option._value;
                random -= option._weight;
            }

            throw(new System.Exception());
        }

        private Option<OptionType>[] _options;
        private int _sumOfWeights;
    }

    public class NonRepeatingWeightedRandomStream<OptionType>
    {
        public NonRepeatingWeightedRandomStream(Option<OptionType>[] inOptions) {
            _options = inOptions;

            int sumOfWeights = 0;
            foreach (Option<OptionType> option in inOptions)
                sumOfWeights += option._weight;

            _shuffledOptionIndices = new int[sumOfWeights];

            int indexToFill_shuffledOptionIndex = 0;
            for (int optionIndex = 0; optionIndex < inOptions.Length; ++optionIndex) {
                int optionWeight = inOptions[optionIndex]._weight;
                for (int weightIndex = 0; weightIndex < optionWeight; ++weightIndex)
                    _shuffledOptionIndices[indexToFill_shuffledOptionIndex++] = optionIndex;
            }

            reset();
        }

        public void reset() {
            for (int index = 0, maxIndex = _shuffledOptionIndices.Length - 1; index < maxIndex; ++index) {
                int swapIndexA = index;
                int swapIndexB = Random.Range(swapIndexA, maxIndex);
                if (swapIndexA != swapIndexB)
                    Utils.swap(ref _shuffledOptionIndices[swapIndexA], ref _shuffledOptionIndices[swapIndexB]);
            }

            _nextOptionIndex = 0;
        }

        public OptionType getNext() {
            if (_nextOptionIndex == _shuffledOptionIndices.Length) {
                reset();
            }

            int index = _shuffledOptionIndices[_nextOptionIndex++];
            return _options[index]._value;
        }

        private Option<OptionType>[] _options;
        private int[] _shuffledOptionIndices;
        private int _nextOptionIndex;
    }
}