<template>
  <div class="task" :class="{done: task.done}">
      <div class="actions">
          <h3 @click="showDescription = !showDescription">{{task.title}}</h3>
          <div class="icons">
              <router-link :to="{name:'UpdateTask', params:{id:task.id}}">
                <span class="material-symbols-outlined">edit</span>
              </router-link>
              <span @click="toggleComplete" class="material-symbols-outlined tick">done</span>
              <span @click="deleteTask" class="material-symbols-outlined delete">delete</span>
          </div>
      </div>
      <div v-if="showDescription" class="details"> 
          <p>{{task.Description}}</p>
      </div>
  </div>
</template>

<script>
import { configuration } from '@/configuration';

export default {
  props:[
      "task"
  ],
  data(){
      return {
          showDescription: false,
          uri: `${configuration.backendBaseUrl}/tasks/${this.task.id}`
      }
  },
  methods:{
      deleteTask(){
          fetch(this.uri, {method:"DELETE"})
              .then(()=>this.$emit('delete', this.task.id))
              .catch(err => console.log(err))
      },
      toggleComplete(){
          fetch(`${this.uri}/complete`, {
              method:"PATCH",
              headers:{'Content-Type': 'application/json'},
              body:JSON.stringify({done: !this.task.done})
          }).then(()=>this.$emit("complete", this.task.id))
          .catch(err => console.log(err))
      }
  }
}
</script>

<style>
.task {
margin: 20px auto;
background: #2f4765;
padding: 10px 20px;
border-radius: 5px;
box-shadow: 0 0px 5px 4px #ea4f30;
}
h3 {
cursor: pointer;
}
.actions {
display: flex;
justify-content: space-between;
align-items: center;
}
.material-symbols-outlined {
font-size: 24px;
margin-left: 10px;
color: #bbb;
cursor: pointer;
}
.material-symbols-outlined:hover {
color: #777;
}
.delete:hover {
color: #ea4f30;
}
.task.done {
box-shadow: 0 0px 5px 4px rgb(43,206,135);
}
.task.done .tick {
color: #35df90;
}
</style>