// Copyright 2015-2019 Miguel Fernandez Arce - All rights reserved
#pragma once

#include <shared_mutex>

#include "Base.h"
#include "Types.h"
#include "FlatHashMap/bytell_hash_map.hpp"
#include "StringKey.h"


extern "C"
{
namespace IdStrings
{
	// Forward Declaration
	struct IdString;

	class IdTable
	{
		friend IdString;

		using Container     = ska::bytell_hash_set<StringKey>;
		using Iterator      = Container::iterator;
		using ConstIterator = Container::const_iterator;

		Container table;
		// Allows multiple reads but locks for new registries
		mutable std::shared_mutex editTableMutex;


		IdTable() = default;

	public:

		size_t Register(const String& string);

		const String& Find(size_t hash) const
		{
			// Ensure no other thread is editing the table
			std::shared_lock lock{ editTableMutex };
			return table.find({ hash })->GetString();
		}

		static IdTable& GetGlobal() {
			static IdTable global{};
			return global;
		}
	};

	/**
	 * An string identified by id.
	 * Searching, comparing and other operations are way cheaper, but creating (indexing) is slightly more expensive.
	 */
	struct DLL_API IdString
	{
	private:

		using Id = size_t;
		Id id;


	public:

		IdString() : id{ Constants::noneId } {}

		IdString(const String& key) {
			id = IdTable::GetGlobal().Register(key);
		}
		IdString(const StringView&& key) : IdString(String{ key }) {}
		IdString(const CharType* key) : IdString(String{ key }) {}
		IdString(const CharType* key, String::size_type size)
			: IdString(String{ key, size })
		{}

		// Copy and Move
		IdString(const IdString& other) = default;
		IdString(IdString&& other) { std::swap(id, other.id); }
		IdString& operator=(const IdString& other) = default;
		IdString& operator=(IdString&& other) { std::swap(id, other.id); return *this; }

		const String& ToString() const
		{
			if (IsNone())
				return Constants::noneStr;
			else
				return IdTable::GetGlobal().Find(id);
		}

		bool operator==(const IdString& other) const { return id == other.id; }

		bool IsNone() const { return id == Constants::noneId; }

		const Id& GetId() const { return id; }


		static const IdString None() {
			static IdString none{ Constants::noneId };
			return none;
		};

	private:

		IdString(const Id& id) : id(id) {}
	};


	// DLL Export

	DLL_API uint64_t RegisterString(const CharType* str, size_t size);

	DLL_API const CharType* FindString(uint64_t hash, size_t& size);
}
}
