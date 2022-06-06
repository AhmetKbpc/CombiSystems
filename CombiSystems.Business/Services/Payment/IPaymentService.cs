using CombiSystems.Core.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Business.Services.Payment
{
    public interface IPaymentService
    {
        InstallmentModel CheckInstallments(string binNumber, decimal price);
        PaymentResponseModel Pay(PaymentModel model);
    }
}
