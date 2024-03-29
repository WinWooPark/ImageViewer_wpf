//win32 api 관련 헤더
#include <windows.h>
#include <afxmt.h>
#include <iostream>

//c++ Thread
#include <thread>
using namespace std;

//
#include <Queue>
#include <mutex>
#include <string>


class MyThread 
{
private:

	bool m_IsThreadRun;

	thread m_Thread; //Thread 객체
	CEvent m_Event; // Thread 동작 이벤트
	mutex m_mut; // Thread 레이스 컨디션 방지, 동기화 객체
	queue<string> m_JobQueue; // 

	void TestThread() 
	{
		string str;

		while (m_IsThreadRun == true)
		{
			::WaitForSingleObject(m_Event, INFINITE);
			m_Event.ResetEvent();

			m_mut.lock();

			str = m_JobQueue.front();

			//openFile();

			//WirteFile(str);


			m_mut.unlock();
			
		}
		return;
	}

public:


	MyThread()
	{

	}

	~MyThread()
	{
		m_Thread.join();
	}

	void StartThread()
	{
		m_Event.ResetEvent();

		m_IsThreadRun = true;
		m_Thread = thread(TestThread);
	}

	void AddString(string str)
	{
		m_mut.lock();

		m_JobQueue.push(str);
		m_Event.SetEvent();

		m_mut.unlock();
	}



};

int main()
{

	MyThread Test;

	Test.StartThread();

	string str = "파일에 저장할 내용";

	Test.AddString(str);

	return 0;
}

