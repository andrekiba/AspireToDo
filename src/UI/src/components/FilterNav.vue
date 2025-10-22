<template>
    <nav class="filter-nav">
        <button @click="updateFilter('all')" :class="{active: current ==='all'}">All</button>
        <button @click="updateFilter('done')" :class="{active: current ==='done'}">Done</button>
        <button @click="updateFilter('todo')" :class="{active: current ==='ongoing'}">ToDo</button>
    </nav>
</template>
<script>
    export default{
        props:['current'],
        methods:{
            updateFilter(by){
                this.$emit('filterChange', by)
            }
        }
    }
</script>
<style>
.filter-nav button {
  background: none;
  border: none;
  color: #bbb;
  outline: none;
  font-size: 12px;
  text-transform: uppercase;
  margin-right: 10px;
  letter-spacing: 1px;
  font-weight: bold;
  cursor: pointer;
}
.filter-nav button.active {
  color: #555;
}
</style>