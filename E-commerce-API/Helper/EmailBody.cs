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
                <p> We’ve received a request to reset the password for your Verdant account. </p>
                <p> Please click on link below to set a new password.</p>
                <a href=""https://theverdantnature.com/#/reset-pwd?email={email}&code={emailToken}"" target=""_blank"" style=""
                    background: #0d6efd; color:white;border-radius: 5px;display:block; margin: auto;width: 50%; text-align
                    :center;text-decoration: none"">Reset Password</a>
                <p>Best Regards, <br>
                    The Verdant Team</p>
            </div>
        </div>
    </body>
    </html>";
        }

    }
}
