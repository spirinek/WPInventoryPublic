<template>
</template>
<script>
export default {
     created(){
              document.title = 'Компьютеры';  
        },
        watch: {
      $route(to,from){
            document.title = to.meta.title || 'Компьютеры';
      }
  }
}
</script>