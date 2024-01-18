﻿using System.ComponentModel;

namespace Nameless.RabbitMQ.Course {
    public enum ExchangeType {
        [Description("direct")]
        Direct = 0,
        [Description("topic")]
        Topic = 1,
        [Description("queue")]
        Queue = 2,
        [Description("fanout")]
        Fanout = 4,
        [Description("headers")]
        Headers = 8
    }
}
