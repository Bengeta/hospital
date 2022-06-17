using Dapper;
using Npgsql;

namespace Lab1;

public static class BDRequests
{
    private const string Connection =
        "User ID=postgres;Password=;Host=localhost;Port=5432;Database=database_name;Pooling=false;Connection Idle Lifetime=10;";

    public static User SignIn(String email, String password)
    {
        using (var connection = new NpgsqlConnection(Connection))
        {
            connection.Open();
            try
            {
                var check = connection.Query<User?>(
                    @"Select name,id From pub.users where email = @email and password = @password limit 1",
                    new {email = email, password = password});
                return check.Count() > 0 ? check.First() : null;
            }
            catch (PostgresException e)
            {
                // Console.WriteLine(e.MessageText);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static int SignUp(String email, String password, String Name)
    {
        using (var connection = new NpgsqlConnection(Connection))
        {
            connection.Open();
            try
            {
                connection.Execute(
                    @"INSERT INTO pub.users(name,password,email) VALUES (@name,@password,@email)",
                    new {name = Name, password = password, email = email});
                return 200;
            }
            catch (PostgresException e)
            {
                //    Console.WriteLine(e.MessageText);
                return 100;
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static List<Patient> GetDayPatients(int day,int id)
    {
        using (var connection = new NpgsqlConnection(Connection))
        {
            connection.Open();
            try
            {
                var list = connection.Query<PatientEntity>(
                    @"select * from pub.patients where day = @day and user_id = @id order by timestart",
                    new {@day = day,@id = id});
                var ans = new List<Patient>();
                foreach (var item in list)
                {
                    var patient = new Patient();
                    patient.id = item.id;
                    patient.day = item.day;
                    patient.name = item.name;
                    patient.timeStart = TimeOnly.FromTimeSpan(item.timeStart);
                    patient.timeEnd = TimeOnly.FromTimeSpan(item.timeEnd);
                    ans.Add(patient);
                }

                return ans;
            }
            catch (PostgresException e)
            {
                //    Console.WriteLine(e.MessageText);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static List<int> GetWeekPatients(int id)
    {
        using (var connection = new NpgsqlConnection(Connection))
        {
            connection.Open();
            try
            {
                var list = connection.Query<DayPatientEntity>(
                    @"SELECT count(*) as patientCount,day FROM pub.patients where user_id = @id GROUP BY day ORDER BY day", new{@id = id});
                var ans = new List<int>() {0, 0, 0, 0, 0, 0, 0};
                foreach (var item in list)
                    ans[item.day - 1] = item.patientCount;
                return ans;
            }
            catch (PostgresException e)
            {
                //  Console.WriteLine(e.MessageText);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static List<PatientEntity> AddPatient(Patient patient,int id)
    {
        using (var connection = new NpgsqlConnection(Connection))
        {
            connection.Open();
            try
            {
                var timeStart = new TimeSpan(patient.timeStart.Hour, patient.timeStart.Minute, 0);
                var timeEnd = new TimeSpan(patient.timeEnd.Hour, patient.timeEnd.Minute, 0);
                var list = connection.Query<PatientEntity>(
                    @"INSERT INTO pub.patients(name,day,timestart,timeend,user_id) VALUES (@name,@day,@timeStart,@timeEnd,@id);
                        select * from pub.patients where day = @day;",
                    new {name = patient.name, day = patient.day, timeStart = timeStart, timeEnd = timeEnd, @id = id});
                return list as List<PatientEntity>;
            }
            catch (PostgresException e)
            {
                //  Console.WriteLine(e.MessageText);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static List<PatientEntity> UpdatePatient(Patient oldPatient, Patient newPatient,int id)
    {
        using (var connection = new NpgsqlConnection(Connection))
        {
            connection.Open();
            try
            {
                var timeStartNew = new TimeSpan(newPatient.timeStart.Hour, newPatient.timeStart.Minute, 0);
                var timeEndNew = new TimeSpan(newPatient.timeEnd.Hour, newPatient.timeEnd.Minute, 0);
                var timeStartOld = new TimeSpan(oldPatient.timeStart.Hour, oldPatient.timeStart.Minute, 0);
                var timeEndOld = new TimeSpan(oldPatient.timeEnd.Hour, oldPatient.timeEnd.Minute, 0);

                var list = connection.Query<PatientEntity>(
                    @"Update pub.patients set name = @new_name, day = @new_day, timestart = @new_timeStart, timeend= @new_timeEnd 
                    where  name = @old_name and day = @old_day and timestart = @old_timeStart and timeend= @old_timeEnd;
                        select * from pub.patients where day = @day;",
                    new
                    {
                        new_name = newPatient.name, new_day = newPatient.day, new_timeStart = timeStartNew,
                        new_timeEnd = timeEndNew,
                        old_name = oldPatient.name, old_day = oldPatient.day, old_timeStart = timeStartOld,
                        old_timeEnd = timeEndOld,
                        day = newPatient.day
                    });
                return list as List<PatientEntity>;
            }
            catch (PostgresException e)
            {
                //  Console.WriteLine(e.MessageText);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static bool DeletePatient(Patient patient)
    {
        using (var connection = new NpgsqlConnection(Connection))
        {
            connection.Open();
            try
            {
                var timeStart = new TimeSpan(patient.timeStart.Hour, patient.timeStart.Minute, 0);
                var timeEnd = new TimeSpan(patient.timeEnd.Hour, patient.timeEnd.Minute, 0);
                connection.Query(
                    @"Delete from pub.patients where  name = @old_name and day = @old_day and timestart = @old_timeStart and timeend= @old_timeEnd;",
                    new
                    {
                        old_name = patient.name, old_day = patient.day, old_timeStart = timeStart,
                        old_timeEnd = timeEnd,
                    });
                return true;
            }
            catch (PostgresException e)
            {
                //  Console.WriteLine(e.MessageText);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}