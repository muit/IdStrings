// Copyright 2015-2019 Miguel Fernandez Arce - All rights reserved
#pragma once

#include <stddef.h> // size_t

#ifdef DLL_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif
