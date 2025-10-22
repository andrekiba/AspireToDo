<template>
  <form @submit.prevent="handleUpdate">
    <label>Title: </label>
    <input type="text" v-model="title" required />
    <label>Description: </label>
    <textarea v-model="description" required></textarea>
    <button>Update Task</button>
  </form>
</template>

<script>
import { configuration } from '@/configuration';
export default {
  props: ["id"],
  data() {
    return {
      title: "",
      description: "",
      uri: `${configuration.backendBaseUrl}/tasks/${this.id}`
    };
  },
  mounted() {
    fetch(this.uri)
      .then((res) => res.json())
      .then((data) => {
        (this.title = data.title), (this.description = data.description);
      });
  },
  methods: {
    handleUpdate() {
      fetch(this.uri, {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          title: this.title,
          description: this.description,
        }),
      })
        .then(() => this.$router.push("/tasks"))
        .catch((err) => console.log(err));
    },
  },
};
</script>

<style>
form {
  background: #2f4765;
  padding: 20px;
  border-radius: 10px;
}
label {
  display: block;
  color: #bbb;
  text-transform: uppercase;
  font-size: 14px;
  font-weight: bold;
  letter-spacing: 1px;
  margin: 20px 0 10px 0;
}
input {
  padding: 10px;
  font-family: "Open Sans", sans-serif;
  font-size: 16px;
  background-color: #2f4765;
  color: #bbb;
  border: 0;
  border-bottom: 2px solid #ddd;
  width: 100%;
  box-sizing: border-box;
}
textarea {
  font-family: "Open Sans", sans-serif;
  font-size: 16px;
  border-style: none;
  background-color: #2f4765;
  border-bottom: 2px solid #ddd;
  color: #bbb;
  padding: 10px;
  width: 100%;
  box-sizing: border-box;
  height: 100px;
}
form button {
  display: block;
  margin: 20px auto 0;
  background: #35df90;
  color: #000;
  padding: 10px;
  border: 0;
  border-radius: 7px;
  font-size: 16px;
}
</style>