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
        List<PaymentTransaction> getPaymentReport(DateTime startDate, DateTime endDate);

        List<PaymentTransaction> getPaymentReportForOrderReferences(List<String> orderReferences);

        List<PaymentTransaction> ﻿getPaymentReportForMandates(List<String> mandateReferences);

        List<ParkingSession> getParkingReport(DateTime startDate, DateTime endDate);

        List<ParkingSession> getParkingReport(DateTime startDate, DateTime endDate, List<String> parkings);

        List<ParkingSession> getParkingReportForBarCodes(List<String> barCodes);

        List<ParkingSession> getParkingReportForTicketNumbers(List<String> ticketNumbers);

        List<ParkingSession> getParkingReportForDTAKeys(List<String> dtaKeys);

        List<ParkingSession> getParkingReportForParkings(List<String> parkings);
    }
}
