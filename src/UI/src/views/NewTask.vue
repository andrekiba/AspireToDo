<template>
  <form @submit.prevent="handleSubmit">
    <label>Title: </label>
    <input type="text" v-model="title" required/>
    <label>Description: </label>
    <textarea v-model="description" required/>
    <button>Create Task</button>
  </form>
</template>

<script>
  import { configuration } from '@/configuration';
  export default{
    data(){
      return{
        title:'',
        description:''
      }
    },
    methods:{
      handleSubmit(){
        let task = {
          title: this.title,
          description: this.description,
          done: false
        }
        fetch(`${configuration.backendBaseUrl}/tasks`,{
          method: 'POST',
          headers: {'Content-Type': 'application/json'},
          body: JSON.stringify(task)
        })
          .then(()=>this.$router.push('/tasks'))
          .catch(err=> console.log(err))
      }
    }
  }
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
  font-family: 'Open Sans', sans-serif;
  font-size: 16px;
  padding: 10px;
  border: 0;
  border-bottom: 2px solid #ddd;
  width: 100%;
  box-sizing: border-box;
  background-color: #2f4765;
  color: #bbb;
}
textarea {
  font-family: 'Open Sans', sans-serif;
  font-size: 16px;
  background-color: #2f4765;
  border: 2px solid #ddd;
  padding: 10px;
  width: 100%;
  box-sizing: border-box;
  height: 100px;
  color: #bbb;
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