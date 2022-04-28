<template v-if="failures">
  <el-container class="new-failure-wrap">
    <newFailure class="new-failure-button"/>
  </el-container>
  <el-container class="failures">
    <failure v-for="failure in failures" :key="failure.id" :failure="failure" />
  </el-container>
</template>
<script lang="js">
import failure from "@/components/technician/failure";
import newFailure from "@/components/technician/newFailure";

export default {
  components: {
    failure,
    newFailure
  },
  data() {
    return {
      failures: null,
    };
  },
  created() {
    this.loadFailures();
  },
  methods: {
    loadFailures() {
      this.failures = null;
      //this.$root.loading = !this.$root.loading
      fetch('api/Failure/GetAll')
          .then(r => r.json())
          .then(json => {
            this.failures = json;
          });
    }
  }
}
</script>
<style scoped>
  .failures {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
  }
  
  .new-failure-wrap {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
  }
  
  .new-failure-button {
    color: white;
    background-color: var(--el-color-primary);;
  }
</style>