<template>
  <div class="tasks">
    <FilterNav @filterChange="current=$event" :current="current"></FilterNav>
    <div v-if="tasks.length">
      <div v-for="task in filterTasks" :key="task.id">
        <SingleTask :task="task"
          @delete="handleDelete"
          @complete="handleComplete">
        </SingleTask>
      </div>
    </div>
  </div>
</template>

<script>
import SingleTask from '../components/SingleTask.vue'
import FilterNav from '@/components/FilterNav.vue'
import { configuration } from '@/configuration';

export default {
  name: 'Tasks',
  components: {
    SingleTask,
    FilterNav
  },
  data(){
    return{
      tasks:[],
      current:'all'
    }
  },
  mounted(){
    fetch(`${configuration.backendBaseUrl}/tasks`)
      .then(res => res.json())
      .then(data => (this.tasks = data))
      .catch(err => console.log(err.message))
  },
  methods:{
    handleDelete(id){
      this.tasks = this.tasks.filter((task) => task.id !== id)
    },
    handleComplete(id){
      let t = this.tasks.find((task) => task.id === id)
      t.done = !t.done 
    }
  },
  computed:{
    filterTasks(){
      if(this.current === 'done'){
        return this.tasks.filter((task) => task.done)
      }
      if(this.current === 'todo'){
        return this.tasks.filter((task) => !task.done)
      }
      return this.tasks
    }
  }
}
</script>
