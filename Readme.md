# CS Blog

![CS Blog Log](Images/ReadMe/CS%20BLOG.png)

A blog api make with aspnet core and ef core

---

## About 🤔

This is an api for a fictional blog called CS blog and, no it's not about the game 😬. This api was made with Asp.net and EntityFramework core frameworks for C# language. I made this to pratice my backend skills and to try a different language 🤨.

In this project you can create 3 differente type of users: Reader, Admin and Writers, each one has different roles in system. Only admin can create a writer or an admin, and both ccan create blog posts. Readers... well, they can read and comment published blog posts 😬.

I implemented a pagination feature, blocking database overload and allowing client to choose how many elements it want in requests.

---

## Technologies 🛠️

- **C#**
- **Mysql**
- **Aspnet core**
- **EntityFramework core**

[![Technologies](https://skills.thijs.gg/icons?i=cs,mysql,dotnet&theme=dark)](https://skills.thijs.gg)

---

## Run it ▶️

**⚠️⚠️⚠️ To run this project you should have Mysql and dotnet installed in your machine ⚠️⚠️⚠️**

**1. Enter in your directory**

```bash
cd YOUR_DIRECTORY
```

**2. Clone this repo**

I recommend to use ssh

```bash
git clone git@github.com:GustavoMont/CSBlog.git
```

**3. Write your db configuration in appsettings.Development.json**

```json
"ConnectionStrings": {
    "Connection": "server=localhost;user=your_user;password=your_password;database=your_database_name"
  },
  //...
```

**4. Run migrations**

```bash
dotnet ef database update
```

or if you have `make` in your machine

```bash
make apply-migrations
```

**5. JUST RUN IT 😁**

```bash
dotnet run
```

or, again, if you have make in yout machinee

**5. JUST RUN IT 😁**

```bash
make run
```

### Initial Instructions

When you run migrations it will create a default admin user. The credentials of default admin is:

```json
{
  "email": "admin@admin.com",
  "password": "123456"
}
```

When authorized, you can change his infos like email, name, last name but you can also reset your password. **I recomend to do it 'cause in real word it should be done.**

After you can create writers and blog posts with admin or create a reader user and test all routes.

---

## DER

![DER](/Images/ReadMe/DER.png)

---
