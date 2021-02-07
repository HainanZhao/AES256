#pragma once

extern "C" __declspec(dllexport) int Add(int num1, int num2);
extern "C" __declspec(dllexport) std::string EncryptAES(const char* password, const char* plain);
extern "C" __declspec(dllexport) std::string DecryptAES(const char* password, const char* encrypted);