using System;
using System.Collections.Generic;
using System.Text;

namespace ColorPickers
{
	public enum IncrementDirection
	{
		Lower,
		Higher
	}

	public enum IncrementAmount
	{
		Small,
		Large
	}

	public enum ColorSpectrumShape
    {
		Box = 0,
		Ring = 1,
    }


	public enum ColorSpectrumComponent
    {
		Hue,
		Saturation,
		Value,

		Red,
		Green,
		Blue,
		Alpha
	}

	enum ColorPickerHsvChannel
	{
		Hue = 0,
		Saturation = 1,
		Value = 2,
		Alpha = 3,
	};

}
