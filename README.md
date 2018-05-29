# Otc.Streaming
[![Build Status](https://travis-ci.org/OleConsignado/otc-streaming.svg?branch=master)](https://travis-ci.org/matheusneder/otc-streaming)

Otc.Streaming.ForwardOnlyPeekableStream provides forward only, read only stream wrapper with peek capability. Originally written to prevent use of `Microsoft.AspNetCore.Http.Internal.BufferingHelper.EnableRewind()` from Microsoft.AspNetCore.Http.dll while reading HttpRequest payload for logging.
