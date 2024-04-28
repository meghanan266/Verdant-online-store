namespace E_commerce_API.Helper
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
    <body style=""margin:0; padding: 0; font-family: Arial, Helvetica, sans-serif;"">
        <div style=""height: auto; background: white no-repeat; width: 400px;padding: 30px"">
            <div>
                <h1> Reset your Password </h1>
                <br>
                <p> You're receiving this e-mail because you requested a password reset for your Verdant account. </p>
                <p> Please click on link below to choose a new password.</p>
                <a href=""http://localhost:4200/#/reset-pwd?email={email}&code={emailToken}"" target=""_blank"" style=""
                    background: #0d6efd; color:white;border-radius: 5px;display:block; margin: auto;width: 50%; text-align
                    :center;text-decoration: none"">Reset Password</a>
                <p>Kind Regards, <br><br>
                    Verdant</p>
            </div>
        </div>
    </body>
    </html>";
        }

    }
}
