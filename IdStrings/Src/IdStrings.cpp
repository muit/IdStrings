// Copyright 2015-2019 Miguel Fernandez Arce - All rights reserved
#include "IdStrings.h"

#include <string>
#include <string.h>


namespace IdStrings
{
	size_t IdTable::Register(const String& string)
	{
		if (string.empty())
		{
			return Constants::noneId;
		}

		// Calculate hash once
		StringKey key{ string };

		ConstIterator FoundIt = table.find(key);
		if (FoundIt != table.end())
		{
			std::shared_lock lock{ editTableMutex };
			return FoundIt->GetHash();
		}
		else
		{
			std::unique_lock lock{ editTableMutex };
			return table.insert(std::move(key)).first->GetHash();
		}
	}


	uint64_t RegisterString(const CharType* str, size_t size)
	{
		return IdTable::GetGlobal().Register({ String{str, size} });
	}

	const CharType* FindString(uint64_t hash, size_t& size)
	{
		const String& str = IdTable::GetGlobal().Find(hash);
		size = str.size();
		return str.c_str();
	}
}
