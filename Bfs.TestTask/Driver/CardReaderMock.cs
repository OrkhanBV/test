using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


namespace Bfs.TestTask.Driver
{
    public class CardDriverMock : ICardDriverMock
    {
        private CardData? _cardData;
        private bool _cantReadCard;
        private bool _triggerCardReaderError;
        private bool _takeCard;

        public void SetCardData(CardData cardData) => _cardData = cardData;

        public void CantReadCard() => _cantReadCard = true;

        public void TriggerCardReaderError() => _triggerCardReaderError = true;

        public void TakeCard() => _takeCard = true;

        public Task<CardData?> ReadCard(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromResult<CardData?>(null);
            }

            if (_triggerCardReaderError)
            {
                return Task.FromResult<CardData?>(null);
            }

            if (_cantReadCard)
            {
                return Task.FromResult<CardData?>(null);
            }

            return Task.FromResult(_cardData);
        }

        public async IAsyncEnumerable<EjectResult> EjectCard(TimeSpan takeCardTimeout, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (_triggerCardReaderError)
            {
                yield return EjectResult.CardReaderError;
                yield break;
            }

            yield return EjectResult.Ejected;

            var delayTask = Task.Delay(takeCardTimeout, cancellationToken);
            var completedTask = await Task.WhenAny(delayTask, Task.Delay(Timeout.Infinite, cancellationToken));

            if (completedTask == delayTask)
            {
                yield return _takeCard ? EjectResult.CardTaken : EjectResult.Retracted;
            }
            else
            {
                yield return EjectResult.Retracted;
            }
        }
    }
}

//

//Determining projects to restore...
//  All projects are up-to-date for restore.
//  Bfs.TestTask -> /Users/orhanbabaev/Projects/testBfsOrkhan/test/Bfs.TestTask/bin/Debug/net8.0/Bfs.TestTask.dll
//Test run for /Users/orhanbabaev/Projects/testBfsOrkhan/test/Bfs.TestTask/bin/Debug/net8.0/Bfs.TestTask.dll (.NETCoreApp, Version = v8.0)
//VSTest version 17.11.0 (arm64)

//Starting test execution, please wait...
//A total of 1 test files matched the specified pattern.
//[xUnit.net 00:00:00.07] Bfs.TestTask.Driver.CardDriverTests.ReadCard_SetCardData_ReturnCardData[FAIL]
//[xUnit.net 00:00:00.07]     Bfs.TestTask.Driver.CardDriverTests.ReadCard_SetCardDataAndCancelTask_ReturnNull[FAIL]
//  Failed Bfs.TestTask.Driver.CardDriverTests.ReadCard_SetCardData_ReturnCardData[< 1 ms]
//  Error Message:
//   Assert.NotNull() Failure: Value is null
//  Stack Trace:
//     at Bfs.TestTask.Driver.CardDriverTests.ReadCard_SetCardData_ReturnCardData() in / Users / orhanbabaev / Projects / testBfsOrkhan / test / Bfs.TestTask / Driver / CardDriverTests.cs:line 15
//--- End of stack trace from previous location ---
//  Failed Bfs.TestTask.Driver.CardDriverTests.ReadCard_SetCardDataAndCancelTask_ReturnNull [1 ms]
//Error Message:
//   Assert.Null() Failure: Value is not null
//Expected: null
//Actual: CardData { CardNumber = 1234 1234 1234 1234 }
//Stack Trace:
//     at Bfs.TestTask.Driver.CardDriverTests.ReadCard_SetCardDataAndCancelTask_ReturnNull() in / Users / orhanbabaev / Projects / testBfsOrkhan / test / Bfs.TestTask / Driver / CardDriverTests.cs:line 43
//--- End of stack trace from previous location ---
//[xUnit.net 00:00:05.08] Bfs.TestTask.Driver.CardDriverTests.EjectCard_TriggerErrorAfterEject_ReturnError[FAIL]
//  Failed Bfs.TestTask.Driver.CardDriverTests.EjectCard_TriggerErrorAfterEject_ReturnError[5 s]
//  Error Message:
//   Assert.Equal() Failure: Values differ
//Expected: CardReaderError
//Actual:   Retracted
//  Stack Trace:
//     at Bfs.TestTask.Driver.CardDriverTests.EjectCard_TriggerErrorAfterEject_ReturnError() in / Users / orhanbabaev / Projects / testBfsOrkhan / test / Bfs.TestTask / Driver / CardDriverTests.cs:line 130
//   at Bfs.TestTask.Driver.CardDriverTests.EjectCard_TriggerErrorAfterEject_ReturnError() in / Users / orhanbabaev / Projects / testBfsOrkhan / test / Bfs.TestTask / Driver / CardDriverTests.cs:line 134
//--- End of stack trace from previous location ---
//[xUnit.net 00:00:15.10] Bfs.TestTask.Driver.CardDriverTests.EjectCard_TakeCardBeforeEjectAndWaitCardRetractDelay_ReturnRetracted[FAIL]
//  Failed Bfs.TestTask.Driver.CardDriverTests.EjectCard_TakeCardBeforeEjectAndWaitCardRetractDelay_ReturnRetracted[5 s]
//  Error Message:
//   Assert.Equal() Failure: Values differ
//Expected: Retracted
//Actual:   CardTaken
//  Stack Trace:
//     at Bfs.TestTask.Driver.CardDriverTests.EjectCard_TakeCardBeforeEjectAndWaitCardRetractDelay_ReturnRetracted() in / Users / orhanbabaev / Projects / testBfsOrkhan / test / Bfs.TestTask / Driver / CardDriverTests.cs:line 191
//   at Bfs.TestTask.Driver.CardDriverTests.EjectCard_TakeCardBeforeEjectAndWaitCardRetractDelay_ReturnRetracted() in / Users / orhanbabaev / Projects / testBfsOrkhan / test / Bfs.TestTask / Driver / CardDriverTests.cs:line 195
//--- End of stack trace from previous location ---

//Failed!  - Failed:     4, Passed: 8, Skipped: 0, Total: 12, Duration: 20 s - Bfs.TestTask.dll(net8.0)