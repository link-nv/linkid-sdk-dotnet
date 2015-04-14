/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDParkingSession
    {
        public DateTime date {get; set;}
        public String barCode { get; set; }
        public String parking { get; set; }
        public String userId { get; set; }
        public double turnover { get; set; }
        public bool validated { get; set; }
        public String paymentOrderReference { get; set; }
        public LinkIDPaymentState paymentState { get; set; }

        public LinkIDParkingSession(DateTime date, String barCode, String parking, String userId, double turnover,
            bool validated, String paymentOrderReference, LinkIDPaymentState paymentState)
        {
            this.date = date;
            this.barCode = barCode;
            this.parking = parking;
            this.userId = userId;
            this.turnover = turnover;
            this.validated = validated;
            this.paymentOrderReference = paymentOrderReference;
            this.paymentState = paymentState;
        }

        public override String ToString()
        {
            String output = "";

            output += "Parking session\n";
            output += "  * date: " + date + "\n";
            output += "  * barCode: " + barCode + "\n";
            output += "  * parking: " + parking + "\n";
            output += "  * userId: " + userId + "\n";
            output += "  * turnover: " + turnover + "\n";
            output += "  * validated: " + validated + "\n";
            output += "  * paymentOrderReference: " + paymentOrderReference + "\n";
            output += "  * paymentState: " + paymentState + "\n";

            return output;
        }
    }
}
