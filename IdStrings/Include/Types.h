// Copyright 2015-2019 Miguel Fernandez Arce - All rights reserved
#pragma once

#include <string>

namespace IdStrings
{
	using CharType = char;
	using StringView = std::basic_string_view<CharType>;
	using String = std::basic_string<CharType, std::char_traits<CharType>, std::allocator<CharType>>;

	struct Constants
	{
		static const String noneStr;
		static const size_t noneId;
	};
}
