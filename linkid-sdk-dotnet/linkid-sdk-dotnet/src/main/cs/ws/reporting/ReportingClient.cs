/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public interface ReportingClient
    {
        void enableLogging();

        /// <summary>
        /// ﻿The payment transactions matching your search. If none found an empty list is returned
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">optional end date, not specified means till now</param>
        /// <returns></returns>
        List<LinkIDPaymentOrder> getPaymentReport(DateTime startDate, DateTime endDate);

        List<LinkIDPaymentOrder> getPaymentReportForOrderReferences(List<String> orderReferences);

        List<LinkIDPaymentOrder> ﻿getPaymentReportForMandates(List<String> mandateReferences);

        List<LinkIDParkingSession> getParkingReport(DateTime startDate, DateTime endDate);

        List<LinkIDParkingSession> getParkingReport(DateTime startDate, DateTime endDate, List<String> parkings);

        List<LinkIDParkingSession> getParkingReportForBarCodes(List<String> barCodes);

        List<LinkIDParkingSession> getParkingReportForTicketNumbers(List<String> ticketNumbers);

        List<LinkIDParkingSession> getParkingReportForDTAKeys(List<String> dtaKeys);

        List<LinkIDParkingSession> getParkingReportForParkings(List<String> parkings);
    }
}
