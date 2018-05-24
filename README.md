# Otc.Streaming
[![Build Status](https://travis-ci.org/matheusneder/otc-streaming.svg?branch=master)](https://travis-ci.org/matheusneder/otc-streaming)

Otc.Streaming.ForwardOnlyPeekableStream provides forward only, read only stream wrapper with peek capability. Originaly created to inspect http requests payload with no use of `HttpRequest.EnableRewind()` from `Microsoft.AspNetCore.Http.Internal.BufferingHelper` (Microsoft.AspNetCore.Http.dll).
