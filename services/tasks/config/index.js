module.exports = {
  "port": process.env.HP_TASKS_PORT,
  "datasource": {
    "host": process.env.HP_DATABASE_HOST,
    "username": process.env.HP_DATABASE_USERNAME,
    "password": process.env.HP_DATABASE_PASSWORD,
    "database": process.env.HP_DATABASE_NAME,
    "debug": process.env.HP_DATABASE_DEBUG || false
  }
}
