// Copyright 2015-2019 Miguel Fernandez Arce - All rights reserved
#pragma once

#include <type_traits>
#include "Types.h"


namespace IdStrings
{
	struct StringKey
	{
	private:
		size_t hash;
		const String str;

	public:
		StringKey(size_t hash = 0) : hash{ hash } {}
		StringKey(String str) : hash{ std::hash<String>()(str) }, str{ std::move(str) } {}

		StringKey(const StringKey& other) : hash{ other.hash } {}
		StringKey(StringKey&& other) : hash{ other.hash }, str{ std::move(other.str) } {}

		StringKey& operator=(const StringKey& other) { hash = other.hash; return *this; }

		const String& GetString() const { return str; }
		const size_t GetHash()    const { return hash; }

		bool operator==(const StringKey& other) const { return hash == other.hash; }
	};
}

namespace std
{
	template <>
	struct hash<IdStrings::StringKey>
	{
		size_t operator()(const IdStrings::StringKey& x) const { return x.GetHash(); }
	};
}
