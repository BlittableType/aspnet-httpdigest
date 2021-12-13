namespace Bt.Web.ServiceClients
{
    public static class Errors
    {
        public static class Common
        {
            public static readonly string NotAuthorized = "not.authorized";
        }

        public static class Authentication 
        {        
            public static readonly string NoToken = "auth.failed.no.token";
            public static readonly string NotAuthorized = "auth.failed.wrong.login.or.password";
            public static readonly string NoHealth = "auth.health.request.failed";
            public static readonly string ServiceFailed = "auth.failed.any.reason";
            public static readonly string SurprisingSuccess = "auth.failed.surprising.success";
        }

        public static class DataProvider 
        {        
            public static readonly string NoData = "data.provider.no.data";
            public static readonly string InvalidQueryCriteria = "data.provider.invalid.query.criteria";
            public static readonly string ServiceFailed = "data.provider.any.reason";
        }

        public static class OrdersExchange 
        {        
            public static readonly string NotAuthorized = "order.exchange.auth.failed";
            public static readonly string OrderIdNotFound = "order.exchange.order.id.not.found";
            public static readonly string OrdersContainsDuplicates = "order.exchange.order.id.not.found";
        }

        public static class OrdersNotify 
        {
            public static readonly string ExpiredTimeInFutureError = "order.notify.exp.time.in.future.error";
            public static readonly string ExpiredTimeCheckFailed = "order.notify.exp.time.check.failed";
            public static readonly string SignatureCheckFailed = "order.notify.sign.check.failed";
            public static readonly string OrdersContainsDuplicates = "order.notify.orders.contains.duplicates";
            public static readonly string OrdersContainsInDb = "order.notify.orders.contains.in.db";
        }

        public static class Db 
        {        
            public static readonly string InsertFailed = "db.insert.failed";
        }
    }
}