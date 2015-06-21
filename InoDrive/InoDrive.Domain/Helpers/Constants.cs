using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Helpers
{
    public static class AppConstants
    {
        public static String USER_NOT_FOUND                 = "Такой пользователь не найден!";
        public static String TRIP_NOT_FOUND                 = "Такая поездка не найдена!";
        public static String USER_HASNT_SUCH_TRIP           = "У пользователя нет такой поездки";
        public static String LETTER_CONFIRM_EMAIL_TITLE     = "Ино Драйв. Подтверждение электронного адреса.";
        public static String LETTER_RESET_PASSWORD_TITLE    = "Ино Драйв. Сброс пароля.";
        public static String INVALID_CONFIRM_EMAIL_CODE     = "Недействительная ссылка для подтверждения почты!";
        public static String INVALID_RESET_PASSWORD_CODE    = "Недействительная ссылка для сброса пароля!";
        public static String EMAIL_WASNT_CONFIRMED          = "Электронный адрес не был подтвержден!";
        public static String EMAIL_WAS_CONFIRMED            = "Электронный адрес был подтвержден!";
        public static String CONFRIM_LETTER_WAS_SENDED      = "Письмо для подтверждения почты было отправлено!";
        public static String CONFRIM_LETTER_WASNT_SENDED    = "Письмо для подтверждения почты не было отправлено!";
        public static String RESET_PASSWORD_WAS_SENDED      = "Письмо для сброса пароля было отправлено!";
        public static String RESET_PASSWORD_WASNT_SENDED    = "Письмо для сброса пароля не было отправлено!";
        public static String PASSWORD_WAS_RESETED           = "Пароль был сменен!";
        public static String PASSWORD_WASNT_RESETED         = "Пароль не был сменен!";
        public static String WRONG_OLD_PASSWORD_VALUE       = "Неверное значение старого пароля!";
        public static String WRONG_OLD_EMAIL_VALUE          = "Неверное значение старого email'a!";
        public static String EMAIL_ALREADY_EXIST            = "Уже зарегистрирован пользователь с таким email'ом!";
        public static String EMAIL_WAS_RESETED              = "Email был сменен!";
        public static String EMAIL_WASNT_RESETED            = "Email не был сменен!";
        public static String NEED_CONFIRM_EMAIL_TO_SIGNIN   = "Вы должны подтвердить email, чтобы выполнить вход!";
        public static String WRONG_PASSWORD                 = "Вы ввели неверный пароль!";
        public static String REGISTRATION_ERROR             = "Произошла ошибка при регистрации пользователя!";
        public static String REGISTRATION_SUCCESS           = "Пользователь был успешно зарегистрирован!";
        public static String TOKEN_WAS_REMOVED              = "Refresh токен был успешно удален!";
        public static String TOKEN_WASNT_REMOVED            = "Произошла ошибка при удалении refresh токена!";
        public static String TRIP_CREATE_ERROR              = "Ошибка при создании поездки!";
        public static String TRIP_REMOVE_ERROR              = "Ошибка при удалении поездки!";
        public static String TRIP_RECOVER_ERROR             = "Ошибка при восстановлении поездки!";
        public static String TRIP_EDIT_ERROR                = "Ошибка при редактировании поездки!";
        public static String TRIP_REMOVE_CAUSE_DATE_ERROR   = "Нельзя удалить уже начавшуюся поездку!";
        public static String PROFILE_EDIT_ERROR             = "Ошибка редактирования профиля!";
        public static String SAME_EMAILS                    = "Старый и новый email'ы не должны совпадать!";
        public static String FIND_TRIPS_ERROR               = "Произошла ошибка при поиске поездок!";

    }
}
